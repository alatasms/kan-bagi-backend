package com.onurcetin.BloodApp.service;

import com.onurcetin.BloodApp.model.Matching;
import com.onurcetin.BloodApp.repository.MatchingRepository;
import java.time.LocalDateTime;
import java.util.List;
import lombok.Generated;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.web.server.ResponseStatusException;

@Service
public class MatchingService {
    private final MatchingRepository matchingRepository;

    public Matching createMatching(String donorId, String postId, String ownerId) {
        if (this.matchingRepository.existsByDonorIdAndPostId(donorId, postId)) {
            throw new ResponseStatusException(HttpStatus.CONFLICT, "A matching with the same donorId and postId already exists.");
        } else {
            Matching matching = new Matching();
            matching.setDonorId(donorId);
            matching.setPostId(postId);
            matching.setOwnerId(ownerId);
            matching.setCreationTime(LocalDateTime.now());
            matching.setLastUpdatedTime(LocalDateTime.now());
            return (Matching)this.matchingRepository.save(matching);
        }
    }

    /** Called by hospital staff (enforced in SecurityConfig) after scanning the donor's QR code. */
    public Matching validateMatching(Long matchingId, String staffId) {
        Matching matching = this.getMatching(matchingId);
        if (Boolean.TRUE.equals(matching.getIsVerified())) {
            throw new ResponseStatusException(HttpStatus.CONFLICT, "This donation has already been verified.");
        }
        LocalDateTime now = LocalDateTime.now();
        matching.setIsVerified(true);
        matching.setVerifiedBy(staffId);
        matching.setVerifiedAt(now);
        matching.setLastUpdatedTime(now);
        return (Matching)this.matchingRepository.save(matching);
    }

    public Matching getMatching(Long matchingId) {
        return (Matching)this.matchingRepository.findById(matchingId)
            .orElseThrow(() -> new ResponseStatusException(HttpStatus.NOT_FOUND, "No such matchingId was found."));
    }

    public boolean existsByDonorIdAndPostId(String donorId, String postId) {
        return this.matchingRepository.existsByDonorIdAndPostId(donorId, postId);
    }

    public List<Matching> getMatchingsByDonorId(String donorId) {
        return this.matchingRepository.findByDonorId(donorId);
    }

    public Matching getMatchingByDonorIdAndPostId(String donorId, String postId) {
        return this.matchingRepository
            .findByDonorIdAndPostId(donorId, postId)
            .orElseThrow(() -> new ResponseStatusException(HttpStatus.NOT_FOUND, "Matching not found for given donorId and postId"));
    }

    @Generated
    public MatchingService(final MatchingRepository matchingRepository) {
        this.matchingRepository = matchingRepository;
    }
}
