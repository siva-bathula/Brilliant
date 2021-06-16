from __future__ import print_function

import os

def rotate(disc, time):
    return (disc['start_pos'] + time + disc['index']) % disc['positions'] == 0

def solve(data, extra_disc=False):
    discs = []
    for row in data:
        row = row.strip('.').split()
        discs.append({
            'positions': int(row[3]),
            'start_pos': int(row[-1]),
            'index': int(row[1][1:])
        })

    if extra_disc:
        discs.append({'positions': 11, 'start_pos': 0, 'index': len(discs) + 1})

    time = 0
    while True:
        for disc in discs:
            if not rotate(disc, time):
                break
        else:  # no break
            return time
        time += 1


if __name__ == '__main__':
    this_dir = os.path.dirname(__file__)
    with open(os.path.join(this_dir, 'data/day15.txt')) as f:
        data = f.read().splitlines()

    print('After', solve(data), 'seconds I can get a capsule.')
    print('After', solve(data, extra_disc=True), 'seconds with a bonus disc I can get a capsule.')