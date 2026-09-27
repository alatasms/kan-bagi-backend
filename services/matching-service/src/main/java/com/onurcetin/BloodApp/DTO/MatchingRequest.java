package com.onurcetin.BloodApp.DTO;

import io.swagger.v3.oas.annotations.media.Schema;
import lombok.Generated;

public class MatchingRequest {
    @Schema(
        description = "ID of the donor",
        example = "12345"
    )
    private String donorId;
    @Schema(
        description = "ID of the blood donation post",
        example = "67890"
    )
    private String postId;
    @Schema(
        description = "ID of the post owner",
        example = "54321"
    )
    private String ownerId;

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
    @Override
    public boolean equals(final Object o) {
        if (o == this) {
            return true;
        } else if (!(o instanceof MatchingRequest other)) {
            return false;
        } else if (!other.canEqual(this)) {
            return false;
        } else {
            Object this$donorId = this.getDonorId();
            Object other$donorId = other.getDonorId();
            if (this$donorId == null ? other$donorId == null : this$donorId.equals(other$donorId)) {
                Object this$postId = this.getPostId();
                Object other$postId = other.getPostId();
                if (this$postId == null ? other$postId == null : this$postId.equals(other$postId)) {
                    Object this$ownerId = this.getOwnerId();
                    Object other$ownerId = other.getOwnerId();
                    return this$ownerId == null ? other$ownerId == null : this$ownerId.equals(other$ownerId);
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
        return other instanceof MatchingRequest;
    }

    @Generated
    @Override
    public int hashCode() {
        int PRIME = 59;
        int result = 1;
        Object $donorId = this.getDonorId();
        result = result * 59 + ($donorId == null ? 43 : $donorId.hashCode());
        Object $postId = this.getPostId();
        result = result * 59 + ($postId == null ? 43 : $postId.hashCode());
        Object $ownerId = this.getOwnerId();
        return result * 59 + ($ownerId == null ? 43 : $ownerId.hashCode());
    }

    @Generated
    @Override
    public String toString() {
        return "MatchingRequest(donorId=" + this.getDonorId() + ", postId=" + this.getPostId() + ", ownerId=" + this.getOwnerId() + ")";
    }
}
