# Helper functions
# OTP generation
# Data validation
# Common utilities 

import random


def generate_otp(length=6):
    """Generate a random OTP of given length."""
    if length <= 0:
        raise ValueError("Length must be a positive integer.")
    return ''.join(str(random.randint(0, 9)) for _ in range(length))
