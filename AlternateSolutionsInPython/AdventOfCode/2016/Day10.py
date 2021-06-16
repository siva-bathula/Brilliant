import collections
import os


def give_value(bots, destination_id, value, full_bots, outputs):
    """Give value to destination_id bot."""
    if 'output' not in destination_id:  # Give to bot
        destination_id = destination_id.replace('bot', '')
        bots[destination_id]['values'] += [value]
        if len(bots[destination_id]['values']) == 2:
            full_bots.append(destination_id)
    else:  # Give to output
        outputs[destination_id] = value

    return bots, full_bots, outputs


def run_bots(bots, bot_id, outputs, full_bots):
    bot = bots[bot_id]

    # Get values
    low, high = sorted(bot['values'])
    high_bot = bot['high']
    low_bot = bot['low']

    bots, full_bots, outputs = give_value(bots, high_bot, high, full_bots, outputs)
    bots, full_bots, outputs = give_value(bots, low_bot, low, full_bots, outputs)

    return bots, outputs, full_bots


def solve(data, comparing=None):
    bots = collections.defaultdict(lambda: {'values': []})
    full_bots = []
    # Get dict of all bots
    for row in data:
        row = row.split()
        if row[0] == 'value':
            # Add to list of values
            bot_id = row[5]
            value = row[1]
            bots[bot_id]['values'] += [int(value)]
            if len(bots[bot_id]['values']) == 2:
                full_bots.append(bot_id)
        elif row[0] == 'bot':
            # Add high & low dest
            bots[row[1]].update({  # Include 'output' string
                'high': row[10] + row[11],
                'low': row[5] + row[6]
            })

    # Run on bots that have both inputs
    outputs = {}
    comparing_bot = None
    comparing_rev = None
    if comparing:
        comparing_rev = list(reversed(comparing))

    for bot_id in full_bots:
        if bots[bot_id]['values'] == comparing or bots[bot_id]['values'] == comparing_rev:
            comparing_bot = bot_id
            break
        bots, outputs, full_bots = run_bots(bots, bot_id, outputs, full_bots)

    if comparing:
        return comparing_bot
    else:
        return outputs['output0'] * outputs['output1'] * outputs['output2']


if __name__ == '__main__':
    this_dir = os.path.dirname(__file__)
    with open(os.path.join(this_dir, 'data/day10.txt')) as f:
        data = f.read().splitlines()

    print('The bot that compares 61 and 17 is', solve(data, comparing=[61, 17]))
    print('The product of outputs 0, 1 & 2 is', solve(data))