import time
import datetime
import msvcrt
import urllib.request
import math
import sys
import gmpy2

from Classes import *
from BrilliantProblems import *
from BrilliantProblems.ComputerScience import LoopTheLoops
from sympy.ntheory import factorint
from sympy import isprime
from decimal import *
from AlternateSolutionsInPython import *
from Classes import Algorithms
from Classes.Algorithms import Manacher
from Classes import Prime
from collections import *
from collections_extended import *
from builtins import len
from sympy import expand, Symbol, Add, Poly
from sympy.abc import x, y , a , b
from math import *
from math import factorial
from sympy import radsimp, sqrt, symbols
from gmpy2 import *

class AlternateSolutionInPython(object):
    x = Symbol('x')
    @staticmethod
    def main(args):
        AlternateSolutionInPython.FactorialDigits(args)

        def wait():
            msvcrt.getch()

    def FactorialDigits(obj):
        ni = 1;
        n = 1000000
        pirt = 2424117
        ert = 2847428
        tenpi = 10 ** 2424117
        tene = 10 ** 2847428
        fact = 1
        while ni <= n :
            ni += 1
            fact *= ni
            fact = fact % tene

        print('2718281th digit : ', (str(fact))[0])
        fact = fact % tenpi
        print('3141592th digit : ', (str(fact))[0])

    def Algebra6thDegreePolynomial(obj):
        fx = expand(2000*x**6 + 100*x**5 + 10*x**3 + x - 2)
        print(expand(fx.subs(x, (a + b**0.5)/y)))

    def ExpandDiscriminant(obj):
        print(expand((12*a**2 + 2*a - 152)**2 - 4*(a**2 - 4*a + 29)*(45*a**2 - 280*a + 480)))

    def IsItPossibleFind5(obj):
        fx = expand(6*x**3 + 3*x**2 -6*2*x - 4)
        gx = expand(fx.subs(x,x -(1/y)))
        print('g(a,b,c): ', gx)
        
    @staticmethod
    def binomialMax(obj):
        f = Factorial.Factorial()
        kMax, binomialValueMax = f.binom_max(1000)
        print(kMax)

    @staticmethod
    def cracking80code(obj):
        #print(Prime.smallest_prime_factor_start_looking_from(2639085015233392202740949386309743259521517793886143240989, 64055443301))
        #print(Prime.smallest_prime_factor(12345))
        #2639085015233392202740949386309743259521517793886143240989
        print ("Current date & time " + time.strftime("%c"))
        print('Starting factorization of 2639085015233392202740949386309743259521517793886143240989')
        #print('is it a prime', isprime(2639085015233392202740949386309743259521517793886143240989))
        #factorsDict = {}
        #factorsDict = factorint(2639085015233392202740949386309743259521517793886143240989)
        #print(factorsDict.keys())

        Prime.fermat(2639085015233392202740949386309743259521517793886143240989, False)
        print ("Current date & time " + time.strftime("%c"))

    @staticmethod
    def manacherTest(obj):
        mObject = Manacher.Manacher()

        text1 = "babcbabcbaccba"
        mObject.findLongestPalindromicString(text1)
  
        text2 = "abaaba"
        mObject.findLongestPalindromicString(text2)
  
        text3 = "abababa"
        mObject.findLongestPalindromicString(text3)
  
        text4 = "abcbabcbabcba"
        mObject.findLongestPalindromicString(text4)
  
        text5 = "forgeeksskeegfor"
        mObject.findLongestPalindromicString(text5)
  
        text6 = "caba"
        mObject.findLongestPalindromicString(text6)
  
        text7 = "abacdfgdcaba"
        mObject.findLongestPalindromicString(text7)
  
        text8 = "abacdfgdcabba"
        mObject.findLongestPalindromicString(text8)
  
        text9 = "abacdedcaba"
        mObject.findLongestPalindromicString(text9)

    @staticmethod
    def biPartitePrimes(obj):
        i=3
        primeCount = 0
        while(i<=10000):
            pn = int("{0:08b}".format(i))
            an = int("{0:08b}".format(i+pn)) - i
            factorsDict = {}
            factorsDict = factorint(an)
            i+=1
            if len(factorint(an)) <= 1:
                primeCount+=1
                print(i," is a bipartite prime. current count : ", primeCount)
        print("Bipartite prime count :", primeCount)

    @staticmethod
    def biPartitePrimes2(obj):
        count=0
        for n in range(3,10001):
            compare=AlternateSolutionInPython.A(n)
            count+=compare>0 and isprime(compare)

        print (count)

    @staticmethod
    def p(n):
        return int(bin(n)[2:])

    @staticmethod
    def A(n):
        return AlternateSolutionInPython.p(n+AlternateSolutionInPython.p(n))-n

    def quickCalc(obj):
        print(1111111111*1111111111)

    def OddTallyCounter(obj):
        sumFi = 0
        for i in range(1,9999999):
            b = 0
            s = setlist()
            while (True):
                b += i
                b %= 10000000
                if s.count(b) == 0: s.add(b)
                else: break
            sumFi += len(s)
        print("Last three digits is :: {0}", (sumFi % 1000))

    def buildpoly1(obj, n):
        return Add(*[ x**i for i in range(0, n+1) ])
    
    def buildpoly2(obj, n):
        return Add(*[ x**i for i in range(1, n+1) ])

    def Quadruples(obj):
        d1 = expand(AlternateSolutionInPython.buildpoly1(obj,1007)**2).as_coefficients_dict()
        print(d1[x**2016])
        
    def ClimbingStairs(obj):
        ways = 0;
        for i in range(5,21):
            d1 = expand(AlternateSolutionInPython.buildpoly2(obj,4)**i).as_coefficients_dict()
            print('i:', i,'Number of ways : ', d1[x**20])
            ways += d1[x**20]
        print('Number of ways : {0}', ways)

    def ApproxRangeOfX(obj):
        x = 12345679000
        while(True):
            n = floor(lgamma(x+1)/(log(x)))
            if(n <= 11814375113):
                break
            else:
                x -= 1
        print('x is : ', str(x))
        print('length of x : ', len(str(x)))

    def LogarithmOfFactorial1(obj):
        content = (urllib.request.urlopen("http://pastebin.com/raw/m1RHfiGr").read()).decode('UTF-8')
        numbers = content.split('\r\n')

        inverseSum = 0;
        for n in numbers:
            inverseSum += AlternateSolutionInPython.LogarithmOfFactorial(obj, int(n), 10)

        print('Sum : ', inverseSum)

    def f(obj, x):
        return (1 - x)/(log(x)) + x + 1/2
        
    def LogarithmOfFactorial(obj, c, nFactor):
        
        n = 2
        while nFactor*AlternateSolutionInPython.f(obj, n) < c:
            n *= 2
        m = n//4
        while m > 1:
            if nFactor*AlternateSolutionInPython.f(obj, n - m) > c: n = n - m
            m = m//2
        return n

    def CheckOutput(obj, a,b):
        if a == 0 and b == 0:
            return 0
        if a == 0: return 1 + AlternateSolutionInPython.CheckOutput(obj, a,b-1)
        if b == 0: return 1 + AlternateSolutionInPython.CheckOutput(obj, a-1,b)

        if a > b:
            return 1 + AlternateSolutionInPython.CheckOutput(obj,AlternateSolutionInPython.CheckOutput(obj,a-b,b),b-1)
        else:
            return 1 + AlternateSolutionInPython.CheckOutput(obj,a-1,AlternateSolutionInPython.CheckOutput(obj,b-a,a))

    def mysterious(obj, L,i,j):
        if i + 1 == j:
            return -100000000000000000000000000

        mid = (i + j) // 2

        x = max(L[i:mid])
        y = max(L[mid:j])
        if x == y:
            return x

        if x > y:
            return max(AlternateSolutionInPython.mysterious(obj, L,i,mid),y)
        else:
            return max(AlternateSolutionInPython.mysterious(obj, L,mid,j),x)

    def MysteriousFunction(obj):
        NumList = [x for x in range(1,11)]
        print('Value: ' + str(AlternateSolutionInPython.mysterious(obj,NumList,0,10)))

    def ExpandPolynomial(obj):
        nd = (-x**4 + 3*x**3 + 2*x**2 + x + 1)
        d1 = expand(nd**8)
        print('d1: ', d1)

    def RadicalSimplify(obj):
        print(radsimp(sqrt(sqrt(2))/sqrt(1+sqrt(3))))

    def SequentialPolynomials(obj):
        f1 = expand(x + 1)
        for n in range(2,11):
            fi = f1.subs(x, x+1)
            fi += x**n
            f1 = expand(fi)
            if(n==10):
                print(f1)

    def S1(obj):
        n = expand(22*(10*x+1)*(16*x+3)*(4*x+1)*(2*x+1))
        num = float(n.subs(x,0.08785388767644))
        d = expand(((10*x+1)**2)*(40*x**2 + 16*x + 3) + 5*(8*x**2+6*x+1)**2)
        den = float(d.subs(x,0.08785388767644))

        print('M: ', num/den)

    def S2(obj):
        fx = expand(2*x**3 + 5*x**2 - 7*x + 1)
        gx = expand(fx.subs(x,x**2+5*x+1))
        print('g(x): ', gx)

    def S3(obj):
        a = expand((1+sqrt(2))**27)
        b = (Poly(a))
        coeff = b.all_coeffs()
        print('a+b+c', coeff[0] + coeff[1] + 2)

    def Expand1(obj):
        fx = expand((x**5 + 20.0*x**3)**5)
        gx = expand((x**7 - 20.0*x**3)**7)
        print(gx - fx)

    def hilarious(obj):
        for n in range(1,50):
            j = 1
            i = 0
            s = 0
            while i < n:
                i += j
                s += 1
                j += 1
            print('n: ',n,', s^2/n: ',float(s*s/n))

    def Hilarious2(obj):
        
        A = mpfr(math.sqrt(6) - math.sqrt(2))
        B = mpfr(math.sqrt(6) + math.sqrt(2))
        am = mpfr(1)
        bm = mpfr(1)
        an = am
        bn = bm
        c = 0
        while(c<2016):
            c+=1
            an = (A*am) - (B*bm)
            bn = (A*bm) + (B*am)
            am = an
            bm = bn

        print('x: ',gmpy2.log2(an*bn))

    if __name__ == '__main__':
        AlternateSolutionInPython.main(sys.argv)