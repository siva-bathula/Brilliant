import os

def draw_rect(grid, width, height):
    for x in range(width):
        for y in range(height):
            grid[y][x] = '#'
    return grid

def rotate_col(grid, column, amount):
    column_list = [grid[x][column] for x in range(len(grid))]
    column_list = column_list[-amount:] + column_list[:-amount]  # Rotate
    for new_val, row in zip(column_list, grid):
        row[column] = new_val
    return grid

def rotate_row(grid, row, amount):
    grid[row] = grid[row][-amount:] + grid[row][:-amount]  # Rotate
    return grid

def solve(data, grid_width, grid_height):
    grid = []
    for _ in range(grid_height):
        grid.append(['.'] * grid_width)

    for instruction in data:
        instruction = instruction.split()
        if instruction[0] == 'rect':
            width, height = instruction[1].split('x')
            width = int(width)
            height = int(height)
            grid = draw_rect(grid, width, height)
        elif instruction[0] == 'rotate':
            if instruction[1] == 'row':
                row_id = int(instruction[2].replace('y=', ''))
                amount = int(instruction[4])
                grid = rotate_row(grid, row_id, amount)
            elif instruction[1] == 'column':
                col_id = int(instruction[2].replace('x=', ''))
                amount = int(instruction[4])
                grid = rotate_col(grid, col_id, amount)

    # Print screen
    for row in grid:
        print(''.join(row))

    # Count lit
    lit = 0
    for row in grid:
        lit += sum(1 for x in row if x == '#')
    return lit

if __name__ == '__main__':
    this_dir = os.path.dirname(__file__)
    with open(os.path.join(this_dir, 'data/day8.txt')) as f:
        data = f.read().splitlines()

    print('The number of lit lights is', solve(data, 50, 6))