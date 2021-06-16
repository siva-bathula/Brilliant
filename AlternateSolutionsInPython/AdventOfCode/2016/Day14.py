from __future__ import print_function

import hashlib
import os
import re

from collections import OrderedDict

def find_triple(data):
    regex = r'(.)\1\1'
    match = re.search(regex, data)
    if match:
        return match.group(1)
    return None

def find_quint(data):
    regex = r'(.)\1\1\1\1'
    match = re.search(regex, data)
    if match:
        return match.group(1)
    return None

def solve(data, lengthen=False):
    salt = data
    index = 0

    print('secret', salt)
    digest = hashlib.md5()
    digest.update(salt.encode('utf8'))

    tracking_keys = OrderedDict()
    actual_keys = []

    while True:
        digest = hashlib.md5((salt + str(index)).encode('utf8'))
        hexrep = digest.hexdigest()

        if lengthen:
            for _ in range(2016):
                digest = hashlib.md5(hexrep.encode('utf8'))
                hexrep = digest.hexdigest()

        quint = find_quint(hexrep)
        if quint:
            # print('found quint', quint, index)
            # print('tracking', tracking_keys)
            for track_index, track_val in list(tracking_keys.items()):
                if track_index + 1000 >= index:
                    if track_val == quint:
                        # print('ACTUAL KEY', track_index)
                        actual_keys.append(index)
                        # print('keys found', len(actual_keys))
                        if len(actual_keys) >= 64:
                            # print('DONE')
                            return track_index
                else:
                    # print('del', track_index)
                    del tracking_keys[track_index]
        triple = find_triple(hexrep)
        if triple:
            # print('hash', hexrep)
            tracking_keys[index] = triple
            # print('tracking', triple, index)

        index += 1


if __name__ == '__main__':
    data = 'cuanljph'

    print('Index', solve(data), 'produces the 64th one-time pad key.')
    print('Index', solve(data, lengthen=True), '64th one-time pad key with stretching.')