package com.onurcetin.BloodApp.controller;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.onurcetin.BloodApp.DTO.BaseResponse;
import com.onurcetin.BloodApp.DTO.DonationFormRequestDTO;
import com.onurcetin.BloodApp.DTO.DonationQuestionDTO;
import com.onurcetin.BloodApp.model.DonationQuestionType;
import com.onurcetin.BloodApp.model.EvaluationForm;
import com.onurcetin.BloodApp.service.FormService;
import java.time.LocalDateTime;
import java.util.Arrays;
import java.util.List;
import java.util.stream.Collectors;
import lombok.Generated;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.server.ResponseStatusException;

/** Each donor has one eligibility form; the donor is always the token subject. */
@RestController
@RequestMapping({"/api/evaluation-form"})
public class EvaluationFormController {
    private final FormService formService;
    private final ObjectMapper objectMapper = new ObjectMapper();

    @GetMapping({"/questions"})
    public ResponseEntity<BaseResponse<List<DonationQuestionDTO>>> getQuestions() {
        List<DonationQuestionDTO> questions = Arrays.stream(DonationQuestionType.values())
            .map(questionType -> new DonationQuestionDTO(questionType.getId(), questionType.name(), questionType.getQuestion(), false))
            .collect(Collectors.toList());
        return ResponseEntity.ok(new BaseResponse<>(questions, true, "200", "Sorular başarıyla getirildi"));
    }

    /** Creates the caller's form, or replaces its answers if it already exists. */
    @PostMapping({"/createForm"})
    public ResponseEntity<BaseResponse<EvaluationForm>> createForm(@RequestBody DonationFormRequestDTO request, Authentication caller) throws JsonProcessingException {
        LocalDateTime now = LocalDateTime.now();
        EvaluationForm form = this.formService.findFormByDonorId(caller.getName()).orElseGet(() -> {
            EvaluationForm created = new EvaluationForm();
            created.setDonorId(caller.getName());
            created.setCreationTime(now);
            return created;
        });
        form.setFormData(toJson(request));
        form.setLastUpdatedTime(now);
        EvaluationForm saved = this.formService.fillForm(form);
        return ResponseEntity.ok(new BaseResponse<>(saved, true, "200", "Form created successfully"));
    }

    @GetMapping({"getForm"})
    public ResponseEntity<BaseResponse<EvaluationForm>> getForm(Authentication caller) {
        return ResponseEntity.ok(new BaseResponse<>(findForm(caller.getName()), true, "200", "Form retrieved successfully"));
    }

    /** Hospital staff and admins only (SecurityConfig): the form of the donor whose QR code was scanned. */
    @GetMapping({"getFormById"})
    public ResponseEntity<BaseResponse<EvaluationForm>> getFormById(@RequestParam("donorId") String donorId) {
        return ResponseEntity.ok(new BaseResponse<>(findForm(donorId), true, "200", "Form retrieved successfully"));
    }

    @PutMapping({"/updateForm"})
    public ResponseEntity<BaseResponse<EvaluationForm>> updateForm(@RequestBody DonationFormRequestDTO request, Authentication caller) throws JsonProcessingException {
        EvaluationForm existingForm = findForm(caller.getName());
        existingForm.setFormData(toJson(request));
        existingForm.setLastUpdatedTime(LocalDateTime.now());
        EvaluationForm updatedForm = this.formService.fillForm(existingForm);
        return ResponseEntity.ok(new BaseResponse<>(updatedForm, true, "200", "Form updated successfully"));
    }

    private EvaluationForm findForm(String donorId) {
        return this.formService.findFormByDonorId(donorId)
            .orElseThrow(() -> new ResponseStatusException(HttpStatus.NOT_FOUND, "Form not found"));
    }

    private String toJson(DonationFormRequestDTO request) throws JsonProcessingException {
        return this.objectMapper.writeValueAsString(request);
    }

    @Generated
    public EvaluationFormController(final FormService formService) {
        this.formService = formService;
    }
}
