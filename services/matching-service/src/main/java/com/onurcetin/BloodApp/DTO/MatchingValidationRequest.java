package com.onurcetin.BloodApp.DTO;

import io.swagger.v3.oas.annotations.media.Schema;
import lombok.Generated;

@Schema(
    description = "Kan bağışı eşleşmesinin doğrulanması için gerekli bilgileri içeren request"
)
public class MatchingValidationRequest {
    @Schema(
        description = "Doğrulanacak eşleşmenin ID'si",
        example = "67890",
        required = true
    )
    private Long matchingId;

    @Generated
    public Long getMatchingId() {
        return this.matchingId;
    }

    @Generated
    public void setMatchingId(final Long matchingId) {
        this.matchingId = matchingId;
    }

    @Generated
    @Override
    public boolean equals(final Object o) {
        if (o == this) {
            return true;
        } else if (!(o instanceof MatchingValidationRequest other)) {
            return false;
        } else if (!other.canEqual(this)) {
            return false;
        } else {
            Object this$matchingId = this.getMatchingId();
            Object other$matchingId = other.getMatchingId();
            return this$matchingId == null ? other$matchingId == null : this$matchingId.equals(other$matchingId);
        }
    }

    @Generated
    protected boolean canEqual(final Object other) {
        return other instanceof MatchingValidationRequest;
    }

    @Generated
    @Override
    public int hashCode() {
        int PRIME = 59;
        int result = 1;
        Object $matchingId = this.getMatchingId();
        return result * 59 + ($matchingId == null ? 43 : $matchingId.hashCode());
    }

    @Generated
    @Override
    public String toString() {
        return "MatchingValidationRequest(matchingId=" + this.getMatchingId() + ")";
    }
}
