import datetime
import time
from Classes import pyecm
from Classes import Prime
from primefac import factorint

print("Current date & time " + time.strftime("%c"))
print('Starting factorization of 2639085015233392202740949386309743259521517793886143240989')
#print(Prime.factors(2639085015233392202740949386309743259521517793886143240989))
#print(list(pyecm.factors(2639085015233392202740949386309743259521517793886143240989, False, True, 10, 1)))

factorint(127534897)

print ("Current date & time " + time.strftime("%c"))

input("Press Enter to continue...")
def wait():
    msvcrt.getch()