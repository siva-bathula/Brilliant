import time
import datetime
import gmpy2 
from gmpy2 import mpz, isqrt
from sympy import isprime

def prime_factors(n):
    i = 2
    factors = []
    while i * i <= n:
        if n % i:
            i += 1
        else:
            n //= i
            factors.append(i)
    if n > 1:
        factors.append(n)
    return factors

def largest_prime_factor(n):
    i = 2
    while i * i <= n:
        if n % i:
            i += 1
        else:
            n //= i
    return n

def smallest_prime_factor(n):
    i = 2
    s = 0
    while i * i < n:
         if n % i == 0:
             s = i
         i = i + 1

    return (s)

# i is greater than 2 and 3.
def smallest_prime_factor_start_looking_from(n,i):
    print("Trying to factorize: %d%%   \r" %(n))
    print("Starting to look from : %d%%   \r"%(i))
    s = 0

    future = time.time() + 10
    while i * i < n:
         if i % 2 == 0 or i % 3 == 0: 
             continue
         if n % i == 0:
             s = i
             break
         i = i + 1
         if time.time() >= future :
             print("Current number : %d%%   \r"%(i))
             future = time.time() + 10
    return (s)

def factors(n):
    n = mpz(n)

    result = set()
    result |= {mpz(1), n}

    def all_multiples(result, n, factor):
        z = n
        f = mpz(factor)
        while z % f == 0:
            result |= {f, z // f}
            f += factor
        return result

    result = all_multiples(result, n, 2)
    result = all_multiples(result, n, 3)

    for i in range(1, isqrt(n) + 1, 6):
        i1 = i + 1
        i2 = i + 5
        if not n % i1:
            result |= {mpz(i1), n // i1}
        if not n % i2:
            result |= {mpz(i2), n // i2}
    return result

def isqrt(n):
  x = n
  y = (x + n // x) // 2
  while y < x:
    x = y
    y = (x + n // x) // 2
  return x

def fermat(n, verbose=True):
    a = isqrt(n) # int(ceil(n**0.5))
    b2 = a*a - n
    b = isqrt(n) # int(b2**0.5)
    count = 0
    while b*b != b2:
        if verbose:
            print('Trying: a=%s b2=%s b=%s' % (a, b2, b))
        a += 1
        
        b2 = a*a - n
        b = isqrt(b2) # int(b2**0.5)
        count += 1
    p=a+b
    q=a-b
    assert n == p * q
    print('a=',a)
    print('b=',b)
    print('p=',p)
    print('q=',q)
    print('pq=',p*q)
    return p, q