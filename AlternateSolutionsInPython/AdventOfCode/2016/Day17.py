from __future__ import print_function

import collections
import hashlib
import os

class State(object):
    """State for a step in the maze."""

    PASSCODE = None
    LONGEST = False

    def __init__(self, x, y, path=''):
        self.x = x
        self.y = y
        self.path = path
        self.priority = priority(x, y, path)

    def __repr__(self):
        return 'State(%s, %s, %s)' % (self.x, self.y, self.path)

    def __eq__(self, other):
        return hash(self) == hash(other)

    def __hash__(self):
        return hash((self.x, self.y, self.path))

    def next_state(self):
        """Generate a child state from here."""
        if self.LONGEST and (self.x, self.y) == (3, 3):
            return
        moves = [(0, -1, 'U'), (0, 1, 'D'), (-1, 0, 'L'), (1, 0, 'R')]
        checksum = hashlib.md5((self.PASSCODE + self.path).encode('utf8'))
        open_map = [c in ('bcdef') for c in checksum.hexdigest()[:4]]

        for is_open, move in zip(open_map, moves):
            if not is_open:
                continue
            x = self.x + move[0]
            y = self.y + move[1]
            if x < 0 or y < 0 or x > 3 or y > 3:  # Out of bounds
                continue
            yield State(x, y, path=self.path + move[2])


def priority(x, y, path):
    """Priority for a State."""
    return len(path)


def solve(data, longest=False):
    State.PASSCODE = data
    State.LONGEST = longest
    goal = (3, 3)
    # Search
    queue = collections.deque()
    starting_state = State(0, 0)
    queue.append(starting_state)

    states = 0
    max_depth = 0
    max_len = 0
    num_valid_paths = 0
    while queue:
        item = queue.popleft()
        if len(item.path) > max_depth:
            max_depth = len(item.path)
            # print('max depth', max_depth, 'states', states, 'len q', len(queue))
        if (item.x, item.y) == goal:
            # print('The number of steps to', goal, 'is', len(item.path))
            if not longest:
                return item.path
            else:
                max_len = max(len(item.path), max_len)
                num_valid_paths += 1
        for new_item in item.next_state():
            queue.append(new_item)
        states += 1

    if longest:
        print(num_valid_paths, 'valid paths generated.')
        return max_len
    else:
        print('No exit found')
        return None


if __name__ == '__main__':
    data = 'qzthpkfp'

    print('The shortest path is', solve(data))
    print('The longest valid path is', solve(data, longest=True))