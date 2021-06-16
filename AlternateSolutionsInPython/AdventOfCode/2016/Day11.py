from __future__ import division, print_function
import collections
import copy
import itertools
import heapq
import os
import win_unicode_console
PRIORITY_Q = False
EQUIV_FUNC = True


class State(object):
    """State for a step moving machines & generators."""
    EQUIV_FUNC = None

    def __init__(self, floors, elevator, parents=0):
        self.floors = floors
        self.elevator = elevator
        self.parents = parents
        self.priority = priority(self.floors, self.elevator)

    def __str__(self):
        return 'State(%s, E: %s, Parents: %s)' % (self.floors, self.elevator, self.parents)

    def __repr__(self):
        return 'State(%s, %s, %s)' % (self.floors, self.elevator, self.parents)

    def __eq__(self, other):
        # Return true if they are equivalent - ie type-rotated
        if self.elevator != other.elevator:
            return False

        return hash(self) == hash(other)

    def __hash__(self):
        if self.EQUIV_FUNC:
            pairs = pairs_rep(self.floors)
            return hash((pairs, self.elevator))
        else:
            frozen_floors = freeze(self.floors)
            return hash((frozen_floors, self.elevator))

    def next_state(self):
        """Generate a child state from here."""
        # Don't move into empty floors below
        empty_below = -1
        for i, floor in enumerate(self.floors):
            if not floor:
                empty_below = i
            else:
                break

        # Move 2 items
        for item, item2 in itertools.combinations(self.floors[self.elevator], 2):
            for dx in (1, -1):
                new_elevator = self.elevator + dx
                if new_elevator not in range(4) or new_elevator <= empty_below:
                    continue
                # Move item
                new_floors = copy.deepcopy(self.floors)
                new_floors[self.elevator].remove(item)
                new_floors[new_elevator].append(item)
                new_floors[self.elevator].remove(item2)
                new_floors[new_elevator].append(item2)
                if valid_state(new_floors):
                    yield State(new_floors, new_elevator, parents=self.parents + 1)
        # Move item
        for item in self.floors[self.elevator]:
            for dx in (1, -1):
                new_elevator = self.elevator + dx
                if new_elevator not in range(4) or new_elevator <= empty_below:
                    continue
                # Move item
                new_floors = copy.deepcopy(self.floors)
                new_floors[self.elevator].remove(item)
                new_floors[new_elevator].append(item)
                if valid_state(new_floors):
                    yield State(new_floors, new_elevator, parents=self.parents + 1)

def freeze(floors):
    """Freeze floors so they can be hashed."""
    return tuple(frozenset(f) for f in floors)

def pairs_rep(floors):
    pairs = list()
    # Generate pairs
    for i, floor in enumerate(floors):
        for item in floor:
            if item[1] == 'M':
                match = item[0] + "G"
                for j, search_floor in enumerate(floors):
                    if match in search_floor:
                        pairs.append((i, j))
    return tuple(sorted(pairs))

def valid_state(floors):
    """Check if state is valid."""
    for floor in floors:
        machines = set(x[0] for x in floor if x[1] == 'M')
        generators = set(x[0] for x in floor if x[1] == 'G')
        unshielded_machines = machines - generators
        if generators and unshielded_machines:
            return False
    return True

def is_done(floors, elevator):
    """Check if done."""
    if elevator != 3:
        return False
    if not floors[0] and not floors[1] and not floors[2] and floors[3]:
        return True
    return False

def priority(floors, elevator):
    """Priority for a State."""
    priority = 3 - elevator
    for i, floor in enumerate(floors):
        priority += (3 - i) * len(floor)
    return priority


def solve(data, extras=False):
    State.EQUIV_FUNC = EQUIV_FUNC
    print('rotational equivalence', State.EQUIV_FUNC)
    # Search
    if extras:
        print('Add extras')
        data[0] += ['YG', 'YM', 'ZG', 'ZM']
    starting_state = State(floors=data, elevator=0)
    print('starting state', starting_state)

    if PRIORITY_Q:
        print('priority q')
        queue = []
        heapq.heappush(queue, (starting_state.priority, starting_state))
    else:
        print('deque')
        queue = collections.deque()
        queue.append(starting_state)

    ever_seen = set()
    ever_seen.add(starting_state)

    states = 0
    max_depth = 0
    while queue:
        if PRIORITY_Q:
            _, item = heapq.heappop(queue)
        else:
            item = queue.popleft()
        # print('popped', item)
        if item.parents > max_depth:
            max_depth = item.parents
            print('max depth', max_depth, 'states', states, 'len q', len(queue))
        if is_done(item.floors, item.elevator):
            print('The number of steps to move everything is', item.parents)
            return item.parents
        for new_item in item.next_state():
            if new_item not in ever_seen:
                # print('added', new_item)
                if PRIORITY_Q:
                    heapq.heappush(queue, (new_item.priority, new_item))
                else:
                    queue.append(new_item)
                ever_seen.add(new_item)
        states += 1

    print('fallthrough')
    return None


if __name__ == '__main__':
    win_unicode_console.enable()
    this_dir = os.path.dirname(__file__)
    with open(os.path.join(this_dir, 'data/day11.txt')) as f:
        data = f.read()
    data = [
        ['AG', 'AM'],
        ['BG', 'CG', 'DG', 'EG'],
        ['BM', 'CM', 'DM', 'EM'],
        [],
    ]
    # data = [
    #     ['AG', 'AM', 'BG', 'CG'],
    #     ['BM', 'CM'],
    #     ['DG', 'DM', 'EG', 'EM'],
    #     [],
    # ]  # 31
    # data = [
    #     ['AG', 'BG', 'BM', 'CG', 'DG', 'DM', 'EG', 'EM'],
    #     ['AM', 'CM'],
    #     [],
    #     [],
    # ]  # 47
    # data = [
    #     ['AM', 'AG', 'BM', 'BG'],
    #     ['CM', 'CG', 'DM', 'DG', 'EG'],
    #     ['EM'],
    #     [],
    # ]  # 37

    print('It takes', solve(data), 'steps to move all 10 items to the top floor.')
    print('It takes', solve(data, extras=True), 'steps to move all 14 items to the top floor.')