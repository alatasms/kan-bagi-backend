import pytest
from app.utils.helpers import generate_otp


# Test cases for generate_otp function
def test_generate_otp_default_length():
    """Test OTP generation with default length."""
    otp = generate_otp()
    assert len(otp) == 6
    assert otp.isdigit()

def test_generate_otp_custom_length():
    """Test OTP generation with custom length."""
    otp = generate_otp(length=8)
    assert len(otp) == 8
    assert otp.isdigit()

def test_generate_otp_invalid_length():
    """Test OTP generation with invalid length. Ensure generate_otp raises ValueError for non-positive length."""
    with pytest.raises(ValueError):
        generate_otp(length=-1)







