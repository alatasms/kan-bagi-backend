package com.onurcetin.BloodApp.service;

import com.onurcetin.BloodApp.model.EvaluationForm;
import com.onurcetin.BloodApp.repository.EvaluationFormRepository;
import java.util.Optional;
import lombok.Generated;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@Transactional
public class FormService {
    private final EvaluationFormRepository evaluationFormRepository;

    public EvaluationForm fillForm(EvaluationForm form) {
        return (EvaluationForm)this.evaluationFormRepository.save(form);
    }

    public Optional<EvaluationForm> readForm(Long formId) {
        return this.evaluationFormRepository.findById(formId);
    }

    public Optional<EvaluationForm> findFormByDonorId(String donorId) {
        return this.evaluationFormRepository.findByDonorId(donorId);
    }

    @Generated
    public FormService(final EvaluationFormRepository evaluationFormRepository) {
        this.evaluationFormRepository = evaluationFormRepository;
    }
}
