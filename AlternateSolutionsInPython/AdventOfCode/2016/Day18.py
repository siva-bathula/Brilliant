from __future__ import print_function

import os


def tile_state(left, center, right):
    if left != right:
        return '^'
    return '.'

def solve(data, height=40):
    row = list(data)
    safe = data.count('.')

    for _ in range(1, height):
        new_row = []
        for l, c, r in zip(['.'] + row[:-1], row, row[1:] + ['.']):
            new_row.append(tile_state(l, c, r))
        safe += new_row.count('.')

        row = new_row

    return safe


if __name__ == '__main__':
    data = '.^^..^...^..^^.^^^.^^^.^^^^^^.^.^^^^.^^.^^^^^^.^...^......^...^^^..^^^.....^^^^^^^^^....^^...^^^^..^'

    print('There are', solve(data, 40), 'safe tiles in 40 rows.')
    print('There are', solve(data, 400000), 'safe tiles in 400000 rows.')