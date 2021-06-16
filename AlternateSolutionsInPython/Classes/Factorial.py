class Factorial(object):
    """description of class"""
    def __init__(self):
        self.data = []

    def factorial(self,n):
        product = 1
        for i in range(n):
            product = product * (i+1)
        return product

    def combination(self,n,k):
        combo = self.factorial(n)/(self.factorial(k) * self.factorial (n-k))
        return combo

    def binom_max(self,n):
        binom_maximum = 0
        k_max = 0
        for j in range(n+1):
            comb_val = self.combination(n-j, j)
            if comb_val >= binom_maximum:
                binom_maximum = comb_val
                k_max = j
        return k_max, binom_maximum