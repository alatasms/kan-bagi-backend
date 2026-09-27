package com.onurcetin.BloodApp.model;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import jakarta.persistence.UniqueConstraint;
import java.time.LocalDateTime;
import lombok.Generated;

@Entity
@Table(
    name = "evaluation_forms",
    uniqueConstraints = {@UniqueConstraint(
        columnNames = {"donorId"}
    )}
)
public class EvaluationForm {
    @Id
    @GeneratedValue(
        strategy = GenerationType.IDENTITY
    )
    private Long formId;
    private String donorId;
    private Long matchingId;
    @Column(
        columnDefinition = "TEXT"
    )
    private String formData;
    private LocalDateTime creationTime;
    private LocalDateTime lastUpdatedTime;

    @Generated
    public Long getFormId() {
        return this.formId;
    }

    @Generated
    public String getDonorId() {
        return this.donorId;
    }

    @Generated
    public Long getMatchingId() {
        return this.matchingId;
    }

    @Generated
    public String getFormData() {
        return this.formData;
    }

    @Generated
    public LocalDateTime getCreationTime() {
        return this.creationTime;
    }

    @Generated
    public LocalDateTime getLastUpdatedTime() {
        return this.lastUpdatedTime;
    }

    @Generated
    public void setFormId(final Long formId) {
        this.formId = formId;
    }

    @Generated
    public void setDonorId(final String donorId) {
        this.donorId = donorId;
    }

    @Generated
    public void setMatchingId(final Long matchingId) {
        this.matchingId = matchingId;
    }

    @Generated
    public void setFormData(final String formData) {
        this.formData = formData;
    }

    @Generated
    public void setCreationTime(final LocalDateTime creationTime) {
        this.creationTime = creationTime;
    }

    @Generated
    public void setLastUpdatedTime(final LocalDateTime lastUpdatedTime) {
        this.lastUpdatedTime = lastUpdatedTime;
    }

    @Generated
    @Override
    public boolean equals(final Object o) {
        if (o == this) {
            return true;
        } else if (!(o instanceof EvaluationForm other)) {
            return false;
        } else if (!other.canEqual(this)) {
            return false;
        } else {
            Object this$formId = this.getFormId();
            Object other$formId = other.getFormId();
            if (this$formId == null ? other$formId == null : this$formId.equals(other$formId)) {
                Object this$matchingId = this.getMatchingId();
                Object other$matchingId = other.getMatchingId();
                if (this$matchingId == null ? other$matchingId == null : this$matchingId.equals(other$matchingId)) {
                    Object this$donorId = this.getDonorId();
                    Object other$donorId = other.getDonorId();
                    if (this$donorId == null ? other$donorId == null : this$donorId.equals(other$donorId)) {
                        Object this$formData = this.getFormData();
                        Object other$formData = other.getFormData();
                        if (this$formData == null ? other$formData == null : this$formData.equals(other$formData)) {
                            Object this$creationTime = this.getCreationTime();
                            Object other$creationTime = other.getCreationTime();
                            if (this$creationTime == null ? other$creationTime == null : this$creationTime.equals(other$creationTime)) {
                                Object this$lastUpdatedTime = this.getLastUpdatedTime();
                                Object other$lastUpdatedTime = other.getLastUpdatedTime();
                                return this$lastUpdatedTime == null ? other$lastUpdatedTime == null : this$lastUpdatedTime.equals(other$lastUpdatedTime);
                            } else {
                                return false;
                            }
                        } else {
                            return false;
                        }
                    } else {
                        return false;
                    }
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
        return other instanceof EvaluationForm;
    }

    @Generated
    @Override
    public int hashCode() {
        int PRIME = 59;
        int result = 1;
        Object $formId = this.getFormId();
        result = result * 59 + ($formId == null ? 43 : $formId.hashCode());
        Object $matchingId = this.getMatchingId();
        result = result * 59 + ($matchingId == null ? 43 : $matchingId.hashCode());
        Object $donorId = this.getDonorId();
        result = result * 59 + ($donorId == null ? 43 : $donorId.hashCode());
        Object $formData = this.getFormData();
        result = result * 59 + ($formData == null ? 43 : $formData.hashCode());
        Object $creationTime = this.getCreationTime();
        result = result * 59 + ($creationTime == null ? 43 : $creationTime.hashCode());
        Object $lastUpdatedTime = this.getLastUpdatedTime();
        return result * 59 + ($lastUpdatedTime == null ? 43 : $lastUpdatedTime.hashCode());
    }

    @Generated
    @Override
    public String toString() {
        return "EvaluationForm(formId="
            + this.getFormId()
            + ", donorId="
            + this.getDonorId()
            + ", matchingId="
            + this.getMatchingId()
            + ", formData="
            + this.getFormData()
            + ", creationTime="
            + this.getCreationTime()
            + ", lastUpdatedTime="
            + this.getLastUpdatedTime()
            + ")";
    }

    @Generated
    public EvaluationForm() {
    }

    @Generated
    public EvaluationForm(
        final Long formId,
        final String donorId,
        final Long matchingId,
        final String formData,
        final LocalDateTime creationTime,
        final LocalDateTime lastUpdatedTime
    ) {
        this.formId = formId;
        this.donorId = donorId;
        this.matchingId = matchingId;
        this.formData = formData;
        this.creationTime = creationTime;
        this.lastUpdatedTime = lastUpdatedTime;
    }
}
