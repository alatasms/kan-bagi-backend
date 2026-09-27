package com.onurcetin.BloodApp.event;

public class PostStatusUpdateEvent {
    private String postId;
    private String status;

    public String getPostId() {
        return this.postId;
    }

    public void setPostId(String postId) {
        this.postId = postId;
    }

    public String getStatus() {
        return this.status;
    }

    public void setStatus(String status) {
        this.status = status;
    }
}
