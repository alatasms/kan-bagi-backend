package com.onurcetin.BloodApp.model;

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
    name = "matchings",
    uniqueConstraints = {@UniqueConstraint(
        columnNames = {"donorId", "postId"}
    )}
)
public class Matching {
    @Id
    @GeneratedValue(
        strategy = GenerationType.IDENTITY
    )
    private Long matchingId;
    private String donorId;
    private String postId;
    private String ownerId;
    private Boolean isVerified = false;
    private LocalDateTime creationTime;
    private LocalDateTime lastUpdatedTime;
    // Hospital staff member (token subject) who confirmed the donation, and when.
    private String verifiedBy;
    private LocalDateTime verifiedAt;

    @Generated
    public Long getMatchingId() {
        return this.matchingId;
    }

    @Generated
    public String getDonorId() {
        return this.donorId;
    }

    @Generated
    public String getPostId() {
        return this.postId;
    }

    @Generated
    public String getOwnerId() {
        return this.ownerId;
    }

    @Generated
    public Boolean getIsVerified() {
        return this.isVerified;
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
    public void setMatchingId(final Long matchingId) {
        this.matchingId = matchingId;
    }

    @Generated
    public void setDonorId(final String donorId) {
        this.donorId = donorId;
    }

    @Generated
    public void setPostId(final String postId) {
        this.postId = postId;
    }

    @Generated
    public void setOwnerId(final String ownerId) {
        this.ownerId = ownerId;
    }

    @Generated
    public void setIsVerified(final Boolean isVerified) {
        this.isVerified = isVerified;
    }

    @Generated
    public void setCreationTime(final LocalDateTime creationTime) {
        this.creationTime = creationTime;
    }

    @Generated
    public void setLastUpdatedTime(final LocalDateTime lastUpdatedTime) {
        this.lastUpdatedTime = lastUpdatedTime;
    }

    public String getVerifiedBy() {
        return this.verifiedBy;
    }

    public void setVerifiedBy(final String verifiedBy) {
        this.verifiedBy = verifiedBy;
    }

    public LocalDateTime getVerifiedAt() {
        return this.verifiedAt;
    }

    public void setVerifiedAt(final LocalDateTime verifiedAt) {
        this.verifiedAt = verifiedAt;
    }

    @Generated
    @Override
    public boolean equals(final Object o) {
        if (o == this) {
            return true;
        } else if (!(o instanceof Matching other)) {
            return false;
        } else if (!other.canEqual(this)) {
            return false;
        } else {
            Object this$matchingId = this.getMatchingId();
            Object other$matchingId = other.getMatchingId();
            if (this$matchingId == null ? other$matchingId == null : this$matchingId.equals(other$matchingId)) {
                Object this$isVerified = this.getIsVerified();
                Object other$isVerified = other.getIsVerified();
                if (this$isVerified == null ? other$isVerified == null : this$isVerified.equals(other$isVerified)) {
                    Object this$donorId = this.getDonorId();
                    Object other$donorId = other.getDonorId();
                    if (this$donorId == null ? other$donorId == null : this$donorId.equals(other$donorId)) {
                        Object this$postId = this.getPostId();
                        Object other$postId = other.getPostId();
                        if (this$postId == null ? other$postId == null : this$postId.equals(other$postId)) {
                            Object this$ownerId = this.getOwnerId();
                            Object other$ownerId = other.getOwnerId();
                            if (this$ownerId == null ? other$ownerId == null : this$ownerId.equals(other$ownerId)) {
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
            } else {
                return false;
            }
        }
    }

    @Generated
    protected boolean canEqual(final Object other) {
        return other instanceof Matching;
    }

    @Generated
    @Override
    public int hashCode() {
        int PRIME = 59;
        int result = 1;
        Object $matchingId = this.getMatchingId();
        result = result * 59 + ($matchingId == null ? 43 : $matchingId.hashCode());
        Object $isVerified = this.getIsVerified();
        result = result * 59 + ($isVerified == null ? 43 : $isVerified.hashCode());
        Object $donorId = this.getDonorId();
        result = result * 59 + ($donorId == null ? 43 : $donorId.hashCode());
        Object $postId = this.getPostId();
        result = result * 59 + ($postId == null ? 43 : $postId.hashCode());
        Object $ownerId = this.getOwnerId();
        result = result * 59 + ($ownerId == null ? 43 : $ownerId.hashCode());
        Object $creationTime = this.getCreationTime();
        result = result * 59 + ($creationTime == null ? 43 : $creationTime.hashCode());
        Object $lastUpdatedTime = this.getLastUpdatedTime();
        return result * 59 + ($lastUpdatedTime == null ? 43 : $lastUpdatedTime.hashCode());
    }

    @Generated
    @Override
    public String toString() {
        return "Matching(matchingId="
            + this.getMatchingId()
            + ", donorId="
            + this.getDonorId()
            + ", postId="
            + this.getPostId()
            + ", ownerId="
            + this.getOwnerId()
            + ", isVerified="
            + this.getIsVerified()
            + ", creationTime="
            + this.getCreationTime()
            + ", lastUpdatedTime="
            + this.getLastUpdatedTime()
            + ")";
    }

    @Generated
    public Matching() {
    }

    @Generated
    public Matching(
        final Long matchingId,
        final String donorId,
        final String postId,
        final String ownerId,
        final Boolean isVerified,
        final LocalDateTime creationTime,
        final LocalDateTime lastUpdatedTime
    ) {
        this.matchingId = matchingId;
        this.donorId = donorId;
        this.postId = postId;
        this.ownerId = ownerId;
        this.isVerified = isVerified;
        this.creationTime = creationTime;
        this.lastUpdatedTime = lastUpdatedTime;
    }
}
