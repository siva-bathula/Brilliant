from __future__ import division, print_function
from functools import total_ordering

import heapq
import os

@total_ordering
class State(object):
    """State for a step in the maze."""
    GOAL = None
    FAV_NUM = None
    MAX_STEPS = None

    def __init__(self, x, y, parents=None):
        self.x = x
        self.y = y
        if parents is None:
            self.parents = []
        else:
            self.parents = parents
        self.priority = priority(x, y, self.GOAL)

    def __str__(self):
        return 'State: %s, %s' % (self.x, self.y)

    def __repr__(self):
        return 'State(%s, %s)' % (self.x, self.y)

    def __eq__(self, other):
        return hash(self) == hash(other)

    def __hash__(self):
        return hash((self.x, self.y))

    def next_state(self):
        """Generate a child state from here."""
        moves = [(0, 1), (0, -1), (1, 0), (-1, 0)]
        for move in moves:
            x = self.x + move[0]
            y = self.y + move[1]
            if x < 0 or y < 0:
                continue
            if is_open(x, y, self.FAV_NUM):
                if self.MAX_STEPS is None or len(self.parents) < self.MAX_STEPS:
                    yield State(x, y, parents=self.parents + [self])

    def __lt__(self, other):
        if isinstance(other, self.__class__):
            return self.priority < other.priority
        return NotImplemented
    
    def __gt__(self, other):
        if isinstance(other, self.__class__):
            return self.priority > other.priority
        return NotImplemented

def priority(x, y, goal):
    """Priority for a State."""
    return abs(goal[0] - x) + abs(goal[1] - y)


def is_open(x, y, fav_num):
    """Is this a wall?"""
    number = x * x + 3 * x + 2 * x * y + y + y * y + fav_num
    return bin(number).count('1') % 2 == 0


def solve(fav_num, goal, max_steps=None):
    State.GOAL = goal
    State.FAV_NUM = fav_num
    State.MAX_STEPS = max_steps

    # # Playing
    # grid = []
    # for y in range(7):
    #     row = ['.' if is_open(x,y,fav_num) else '#' for x in range(10)]
    #     grid.append(row)
    # grid[4][7] = 'G'
    # for row in grid:
    #     print(''.join(row))

    # Search
    queue = []
    starting_state = State(1, 1)
    heapq.heappush(queue, (starting_state.priority, starting_state))
    ever_seen = set()
    ever_seen.add(starting_state)
    steps = 0
    max_depth = 0
    while queue:
        priority, item = heapq.heappop(queue)
        if len(item.parents) > max_depth:
            max_depth = len(item.parents)
            # print('max depth', max_depth, 'states', steps, 'len q', len(queue))
        if (item.x, item.y) == goal:
            print('The number of steps to', goal, 'is', len(item.parents))
            return len(item.parents)
        ever_seen.add(item)
        for new_item in item.next_state():
            if new_item not in ever_seen:
                heapq.heappush(queue, (new_item.priority, new_item))
        steps += 1

    print('The number of states we can reach in', max_steps, 'steps is', len(ever_seen))
    return None


if __name__ == '__main__':
    data = int('1362')
    print('It takes', solve(data, goal=(31, 39)), 'steps to reach 31,39')
    print('The most locations that can be reached in 50 steps is', solve(data, goal=(-1, -1), max_steps=50))