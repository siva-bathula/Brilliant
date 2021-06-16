import os

def solve_v2_slow(data):
    # This works, but runs out of memory and is pathologically slow
    decompressed_len = 0
    max_i = len(data)
    i = 0

    while i < max_i:
        if data[i] == '(':
            # Read instruction
            instruction = ''
            i += 1  # Move past opening ()
            while data[i] != ')':
                instruction += data[i]
                i += 1
            i += 1  # Move past closing )

            # Parse instruction
            num_chars, repeat = map(int, instruction.split('x'))

            # Insert new data into data, drop old data
            data = data[i:i+num_chars] * repeat + data[i+num_chars:]
            max_i = len(data)
            i = 0
        else:
            decompressed_len += 1
            i += 1

    return decompressed_len

def solve(data, version=1):
    decompressed_len = 0
    i = 0

    while i < len(data):
        if data[i] == '(':
            # Get instruction
            instruction = ''
            i += 1  # Move past opening ()
            while data[i] != ')':
                instruction += data[i]
                i += 1
            i += 1  # Move past closing )
            num_chars, repeat = map(int, instruction.split('x'))

            # Add to decompressed length
            if version == 1:
                decompressed_len += num_chars * repeat
            elif version == 2:
                decompressed_len += solve(data[i:i+num_chars], version=2) * repeat

            # Walk past parsed data
            i += num_chars
        else:
            decompressed_len += 1
            i += 1

    return decompressed_len


if __name__ == '__main__':
    this_dir = os.path.dirname(__file__)
    with open(os.path.join(this_dir, 'data\day9.txt')) as f:
        data = f.read()

    print('The decompressed length is', solve(data))
    print('The decompressed length with v2 compression is', solve(data,2))