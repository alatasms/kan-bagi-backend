package com.onurcetin.BloodApp.repository;

import com.onurcetin.BloodApp.model.Matching;
import java.util.List;
import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface MatchingRepository extends JpaRepository<Matching, Long> {
    boolean existsByDonorIdAndPostId(String donorId, String postId);

    List<Matching> findByDonorId(String donorId);

    Optional<Matching> findByDonorIdAndPostId(String donorId, String postId);
}
