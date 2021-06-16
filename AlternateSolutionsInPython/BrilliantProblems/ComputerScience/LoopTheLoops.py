import itertools

class LoopTheLoops(object):
    """description of class"""
    def __init__(self):
        self.data = []

    def size(G, x, y):
        q = [(x, y)]
        S = [[0] * len(G[0]) for _ in range(len(G))]
        ans = 0
        while q:
            i,j = q.pop()
            if S[i][j]: continue
            if G[i][j] != G[x][y]: continue
            S[i][j] = 1
            ans += 1
            if i > 0: q.append((i-1, j))
            if j > 0: q.append((i, j-1))
            if i < len(G)-1: q.append((i+1, j))
            if j < len(G[0])-1: q.append((i, j+1))
        return ans

    def check(G):
        r, c = len(G), len(G[0])
        x, y = None, None
        for i in range(r):
            if x != None: break
            for j in range(c):
                if G[i][j] == 1:
                    x, y = i, j
                    break
        if x == None: return False
        Gnew = [[0]*(c+2)] + [[0]+row+[0] for row in G] + [[0]*(c+2)]
        return (LoopTheLoops.size(Gnew, 0, 0) + LoopTheLoops.size(Gnew, x+1, y+1) == (r+2)*(c+2))

    def solve(obj, r, c):
        ct = 0
        for k in itertools.product([0,1], repeat=r*c):
            G = [list(k[i*c:(i+1)*c]) for i in range(r)]
            if LoopTheLoops.check(G): ct += 1
        return ct

    HEIGHT = 3
    WIDTH = 30
    # creates a grid where all cells before (sx,sy) are unavailable
    def createGrid(sx, sy):
            grid = []
            for line in range(sy):
                    grid += [ LoopTheLoops.WIDTH * [False] ]
            grid += [ sx * [False] + (LoopTheLoops.WIDTH - sx) * [True] ]
            for line in range(sy + 1, LoopTheLoops.HEIGHT):
                    grid += [ LoopTheLoops.WIDTH * [True] ]
            return grid

    DIRECTIONS = [(1,0), (0,1), (-1,0), (0,-1)]
    def getNbLoops(sx, sy, px, py, grid, directions = None):
            global DIRECTIONS
            # directions can be given at start to avoid coming directly back to start
            if directions == None:
                    directions = LoopTheLoops.DIRECTIONS

            # copy grid before making changes, otherwise it changes everywhere
            grid = [ t[:] for t in grid ]

            grid[py][px] = False
            nb = 0
            for (dx,dy) in directions:
                    nx = px + dx
                    ny = py + dy
                    # coming back to start?
                    if nx == sx and ny == sy:
                            nb += 1
                    else:
                            if 0 <= nx and nx < LoopTheLoops.WIDTH and 0 <= ny and ny < LoopTheLoops.HEIGHT:
                                    if grid[ny][nx]:
                                            nb += LoopTheLoops.getNbLoops(sx, sy, nx, ny, grid)
            return nb

    def solve2(obj, h, w):
        LoopTheLoops.HEIGHT = h
        LoopTheLoops.WIDTH = w
        # count nb of loops for each possible start
        nb = 0
        for start in range(0, LoopTheLoops.HEIGHT * LoopTheLoops.WIDTH - LoopTheLoops.WIDTH - 1):
                sx = start % LoopTheLoops.WIDTH
                sy = int(start / LoopTheLoops.WIDTH)
                grid = LoopTheLoops.createGrid(sx, sy)
                # Starting on the right, because up and left
                # are no more available, and down is the other way around on the loop
                if sx < LoopTheLoops.WIDTH - 1:
                        nb += LoopTheLoops.getNbLoops(sx, sy, sx+1, sy, grid, [(1,0), (0,1)])

        print('Number of paths: ', nb)