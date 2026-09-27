package com.onurcetin.BloodApp.DTO;

import lombok.Generated;

public class DonationQuestionDTO {
    private int value;
    private String key;
    private String displayname;
    private boolean answer;

    @Generated
    public int getValue() {
        return this.value;
    }

    @Generated
    public String getKey() {
        return this.key;
    }

    @Generated
    public String getDisplayname() {
        return this.displayname;
    }

    @Generated
    public boolean isAnswer() {
        return this.answer;
    }

    @Generated
    public void setValue(final int value) {
        this.value = value;
    }

    @Generated
    public void setKey(final String key) {
        this.key = key;
    }

    @Generated
    public void setDisplayname(final String displayname) {
        this.displayname = displayname;
    }

    @Generated
    public void setAnswer(final boolean answer) {
        this.answer = answer;
    }

    @Generated
    @Override
    public boolean equals(final Object o) {
        if (o == this) {
            return true;
        } else if (!(o instanceof DonationQuestionDTO other)) {
            return false;
        } else if (!other.canEqual(this)) {
            return false;
        } else if (this.getValue() != other.getValue()) {
            return false;
        } else if (this.isAnswer() != other.isAnswer()) {
            return false;
        } else {
            Object this$key = this.getKey();
            Object other$key = other.getKey();
            if (this$key == null ? other$key == null : this$key.equals(other$key)) {
                Object this$displayname = this.getDisplayname();
                Object other$displayname = other.getDisplayname();
                return this$displayname == null ? other$displayname == null : this$displayname.equals(other$displayname);
            } else {
                return false;
            }
        }
    }

    @Generated
    protected boolean canEqual(final Object other) {
        return other instanceof DonationQuestionDTO;
    }

    @Generated
    @Override
    public int hashCode() {
        int PRIME = 59;
        int result = 1;
        result = result * 59 + this.getValue();
        result = result * 59 + (this.isAnswer() ? 79 : 97);
        Object $key = this.getKey();
        result = result * 59 + ($key == null ? 43 : $key.hashCode());
        Object $displayname = this.getDisplayname();
        return result * 59 + ($displayname == null ? 43 : $displayname.hashCode());
    }

    @Generated
    @Override
    public String toString() {
        return "DonationQuestionDTO(value="
            + this.getValue()
            + ", key="
            + this.getKey()
            + ", displayname="
            + this.getDisplayname()
            + ", answer="
            + this.isAnswer()
            + ")";
    }

    @Generated
    public DonationQuestionDTO(final int value, final String key, final String displayname, final boolean answer) {
        this.value = value;
        this.key = key;
        this.displayname = displayname;
        this.answer = answer;
    }

    @Generated
    public DonationQuestionDTO() {
    }
}
