package com.onurcetin.BloodApp.repository;

import com.onurcetin.BloodApp.model.EvaluationForm;
import java.util.Optional;
import org.springframework.data.jpa.repository.JpaRepository;

public interface EvaluationFormRepository extends JpaRepository<EvaluationForm, Long> {
    Optional<EvaluationForm> findByDonorId(String donorId);
}
