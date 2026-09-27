package com.onurcetin.BloodApp.controller;

import com.onurcetin.BloodApp.DTO.BaseResponse;
import com.onurcetin.BloodApp.DTO.MatchingRequest;
import com.onurcetin.BloodApp.DTO.MatchingValidationRequest;
import com.onurcetin.BloodApp.config.Roles;
import com.onurcetin.BloodApp.model.Matching;
import com.onurcetin.BloodApp.service.MatchingService;
import com.onurcetin.BloodApp.service.QrService;
import jakarta.servlet.http.HttpServletResponse;
import java.awt.image.BufferedImage;
import java.io.ByteArrayOutputStream;
import java.util.Base64;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import javax.imageio.ImageIO;
import lombok.Generated;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.server.ResponseStatusException;

/** The caller is always the token subject (Authentication#getName); ids in requests never identify the caller. */
@RestController
@RequestMapping({"/api/matching"})
public class MatchingController {
    private final MatchingService matchingService;
    private final QrService qrService;

    @PostMapping({"/create"})
    public ResponseEntity<BaseResponse<Map<String, String>>> createMatching(@RequestBody MatchingRequest request, Authentication caller) throws Exception {
        // The donor is the caller; a donorId in the body is ignored.
        String donorId = caller.getName();
        String postId = request.getPostId();
        if (this.matchingService.existsByDonorIdAndPostId(donorId, postId)) {
            Matching existingMatching = this.matchingService.getMatchingByDonorIdAndPostId(donorId, postId);
            String message = "donorId and postId are unique and cannot be matched more than once";
            return ResponseEntity.ok(new BaseResponse<>(qrResponse(existingMatching, message), true, "200", message));
        } else {
            Matching matching = this.matchingService.createMatching(donorId, postId, request.getOwnerId());
            String message = "Matching created successfully";
            return ResponseEntity.ok(new BaseResponse<>(qrResponse(matching, message), true, "200", message));
        }
    }

    @GetMapping({"{matchingId}"})
    public ResponseEntity<BaseResponse<Matching>> getMatching(@PathVariable Long matchingId, Authentication caller) {
        Matching matching = this.matchingService.getMatching(matchingId);
        boolean involved = caller.getName().equals(matching.getDonorId()) || caller.getName().equals(matching.getOwnerId());
        if (!involved && !isStaffOrAdmin(caller)) {
            throw new ResponseStatusException(HttpStatus.FORBIDDEN, "You are not allowed to view this matching.");
        }
        return ResponseEntity.ok(new BaseResponse<>(matching, true, "200", "Matching retrieved successfully"));
    }

    /** Hospital staff only (SecurityConfig). */
    @PostMapping({"/validate"})
    public ResponseEntity<BaseResponse<Matching>> validateMatching(@RequestBody MatchingValidationRequest request, Authentication caller) {
        Matching matching = this.matchingService.validateMatching(request.getMatchingId(), caller.getName());
        return ResponseEntity.ok(new BaseResponse<>(matching, true, "200", "Matching validated successfully"));
    }

    @GetMapping({"/generate-qr"})
    public void generateQr(@RequestParam("donorId") String donorId, @RequestParam("postId") String postId, HttpServletResponse response) throws Exception {
        String content = donorId + ":" + postId;
        BufferedImage qrImage = this.qrService.generateQrCode(content);
        response.setContentType("image/png");
        ImageIO.write(qrImage, "PNG", response.getOutputStream());
    }

    @GetMapping({"/user/{userId}"})
    public ResponseEntity<BaseResponse<List<Map<String, Object>>>> getUserMatchings(@PathVariable String userId, Authentication caller) {
        if (!caller.getName().equals(userId) && !isStaffOrAdmin(caller)) {
            throw new ResponseStatusException(HttpStatus.FORBIDDEN, "You can only view your own matchings.");
        }
        List<Matching> matchings = this.matchingService.getMatchingsByDonorId(userId);
        List<Map<String, Object>> responseList = matchings.stream().map(matching -> {
            Map<String, Object> map = new HashMap<>();
            map.put("matchingId", matching.getMatchingId());
            map.put("postId", matching.getPostId());
            map.put("isVerified", matching.getIsVerified());
            map.put("ownerId", matching.getOwnerId());
            map.put("creationTime", matching.getCreationTime());
            map.put("lastUpdatedTime", matching.getLastUpdatedTime());
            return map;
        }).toList();
        return ResponseEntity.ok(new BaseResponse<>(responseList, true, "200", "User matchings retrieved successfully"));
    }

    /** The QR code carries the matching id, which staff scan and send to /validate. */
    private Map<String, String> qrResponse(Matching matching, String message) throws Exception {
        BufferedImage qrImage = this.qrService.generateQrCode(matching.getMatchingId().toString());
        ByteArrayOutputStream baos = new ByteArrayOutputStream();
        ImageIO.write(qrImage, "png", baos);
        String base64Qr = Base64.getEncoder().encodeToString(baos.toByteArray());
        return Map.of("message", message, "matchingId", matching.getMatchingId().toString(), "qrBase64", base64Qr);
    }

    private static boolean isStaffOrAdmin(Authentication caller) {
        return caller.getAuthorities().stream()
            .anyMatch(a -> Roles.HOSPITAL_STAFF.equals(a.getAuthority()) || Roles.ADMIN.equals(a.getAuthority()));
    }

    @Generated
    public MatchingController(final MatchingService matchingService, final QrService qrService) {
        this.matchingService = matchingService;
        this.qrService = qrService;
    }
}
