import os

def cpy(params, registers, pc):
    value, dest = params.split()
    try:
        registers[dest] = int(value)
    except Exception:
        registers[dest] = registers[value]
    return registers, pc + 1

def inc(params, registers, pc):
    dest = params.strip()
    registers[dest] += 1
    return registers, pc + 1

def dec(params, registers, pc):
    dest = params.strip()
    registers[dest] -= 1
    return registers, pc + 1

def jnz(params, registers, pc):
    register, distance = params.split()
    try:
        value = int(register)
    except Exception:
        value = registers[register]
    if value != 0:
        return registers, pc + int(distance)
    else:
        return registers, pc + 1

def solve(data, c=0):
    instruction_set = {
        'cpy': cpy,
        'inc': inc,
        'dec': dec,
        'jnz': jnz,
    }
    registers = {'a': 0, 'b': 0, 'c': c, 'd': 0}

    pc = 0
    while True:
        try:
            instruction = data[pc]
        except IndexError:
            break
        action = instruction[:3]
        registers, pc = instruction_set[action](instruction[4:], registers, pc)

    return registers


if __name__ == '__main__':
    this_dir = os.path.dirname(__file__)
    with open(os.path.join(this_dir, 'data/day12.txt')) as f:
        data = f.read().splitlines()

    print('The value in register a is', solve(data)['a'])
    print('The value in register a with c=1 is', solve(data, c=1)['a'])