from __future__ import print_function

import os


def dragon_curve(data):
    rev_data = ''.join('0' if x == '1' else '1' for x in reversed(data))
    return data + '0' + rev_data

def calc_checksum(data):
    checksum = []
    for i in range(0, len(data), 2):
        x, y = data[i], data[i+1]
        if x == y:
            checksum.append('1')
        else:
            checksum.append('0')
    return ''.join(checksum)

def solve(data, length):
    fill_data = data
    # Generate data
    while len(fill_data) < length:
        fill_data = dragon_curve(fill_data)

    # Truncate
    truncated = fill_data[:length]

    # Generate checksum
    checksum = calc_checksum(truncated)
    while len(checksum) % 2 == 0:
        checksum = calc_checksum(checksum)

    return checksum


if __name__ == '__main__':
    data = '10111011111001111'

    print('The correct checksum for size 272 disk is', solve(data, 272))
    print('The correct checksum for size 35651584 disk is', solve(data, 35651584))