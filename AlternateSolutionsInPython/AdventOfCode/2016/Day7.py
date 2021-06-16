import re
import os

import regex

def split_ip(ip):
    # Split based on []
    split = re.split(r'\[|\]', ip)
    # Divide into inside & outside []
    outside = split[::2]
    inside = split[1::2]

    return outside, inside

def support_tls(ip):
    outside, inside = split_ip(ip)

    # Don't match 4 in a row, but match abba
    abba_regex = r'(?!(\w)\1\1\1)(\w)(\w)\3\2'
    # Find any abba outside []
    abba_flag = False
    for o in outside:
        match = re.search(abba_regex, o)
        if match:
            abba_flag = True
            break

    # Check for no abba inside []
    inside_flag = False
    for i in inside:
        match = re.search(abba_regex, i)
        if match:
            inside_flag = True
            break

    if abba_flag and not inside_flag:
        return True
    return False

def support_ssl(ip):
    outside, inside = split_ip(ip)

    # Match three where the first and last are the same
    aba_regex = r'(\w)(\w)\1'

    # Find all possible aba matches
    aba_matches = []
    for o in outside:
        # Need to find overlapping matches
        overlapping_matches = regex.findall(aba_regex, o, overlapped=True)
        for match in overlapping_matches:
            if match[0] != match[1]:
                aba_matches.append(match)

    # Look for a bab in each inside segment
    for i in inside:
        # Check each aba match
        for aba in aba_matches:
            bab = aba[1] + aba[0] + aba[1]
            if bab in i:
                return True

    return False

def solve(data, ssl=False):
    if not ssl:
        return sum(support_tls(ip) for ip in data)
    else:
        return sum(support_ssl(ip) for ip in data)


if __name__ == '__main__':
    this_dir = os.path.dirname(__file__)
    with open(os.path.join(this_dir, 'data/day7.txt')) as f:
        data = f.read().splitlines()

    print(solve(data), 'IPs support TLS')
    print(solve(data, ssl=True), 'IPs support SSL')