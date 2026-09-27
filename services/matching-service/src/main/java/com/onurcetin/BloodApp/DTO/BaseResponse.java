package com.onurcetin.BloodApp.DTO;

import lombok.Generated;

public class BaseResponse<T> {
    private T response;
    private boolean isSuccess;
    private String resultCode;
    private String resultMessage;

    @Generated
    public T getResponse() {
        return this.response;
    }

    @Generated
    public boolean isSuccess() {
        return this.isSuccess;
    }

    @Generated
    public String getResultCode() {
        return this.resultCode;
    }

    @Generated
    public String getResultMessage() {
        return this.resultMessage;
    }

    @Generated
    public void setResponse(final T response) {
        this.response = response;
    }

    @Generated
    public void setSuccess(final boolean isSuccess) {
        this.isSuccess = isSuccess;
    }

    @Generated
    public void setResultCode(final String resultCode) {
        this.resultCode = resultCode;
    }

    @Generated
    public void setResultMessage(final String resultMessage) {
        this.resultMessage = resultMessage;
    }

    @Generated
    @Override
    public boolean equals(final Object o) {
        if (o == this) {
            return true;
        } else if (!(o instanceof BaseResponse<?> other)) {
            return false;
        } else if (!other.canEqual(this)) {
            return false;
        } else if (this.isSuccess() != other.isSuccess()) {
            return false;
        } else {
            Object this$response = this.getResponse();
            Object other$response = other.getResponse();
            if (this$response == null ? other$response == null : this$response.equals(other$response)) {
                Object this$resultCode = this.getResultCode();
                Object other$resultCode = other.getResultCode();
                if (this$resultCode == null ? other$resultCode == null : this$resultCode.equals(other$resultCode)) {
                    Object this$resultMessage = this.getResultMessage();
                    Object other$resultMessage = other.getResultMessage();
                    return this$resultMessage == null ? other$resultMessage == null : this$resultMessage.equals(other$resultMessage);
                } else {
                    return false;
                }
            } else {
                return false;
            }
        }
    }

    @Generated
    protected boolean canEqual(final Object other) {
        return other instanceof BaseResponse;
    }

    @Generated
    @Override
    public int hashCode() {
        int PRIME = 59;
        int result = 1;
        result = result * 59 + (this.isSuccess() ? 79 : 97);
        Object $response = this.getResponse();
        result = result * 59 + ($response == null ? 43 : $response.hashCode());
        Object $resultCode = this.getResultCode();
        result = result * 59 + ($resultCode == null ? 43 : $resultCode.hashCode());
        Object $resultMessage = this.getResultMessage();
        return result * 59 + ($resultMessage == null ? 43 : $resultMessage.hashCode());
    }

    @Generated
    @Override
    public String toString() {
        return "BaseResponse(response="
            + this.getResponse()
            + ", isSuccess="
            + this.isSuccess()
            + ", resultCode="
            + this.getResultCode()
            + ", resultMessage="
            + this.getResultMessage()
            + ")";
    }

    @Generated
    public BaseResponse() {
    }

    @Generated
    public BaseResponse(final T response, final boolean isSuccess, final String resultCode, final String resultMessage) {
        this.response = response;
        this.isSuccess = isSuccess;
        this.resultCode = resultCode;
        this.resultMessage = resultMessage;
    }
}
