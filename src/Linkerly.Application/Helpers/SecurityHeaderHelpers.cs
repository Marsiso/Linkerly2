using Microsoft.AspNetCore.Builder;

namespace Linkerly.Application.Helpers;

public static class SecurityHeaderHelpers
{
    public static HeaderPolicyCollection GetHeaderPolicyCollection()
    {
        var headerPolicyCollection = new HeaderPolicyCollection();

        headerPolicyCollection.AddFrameOptionsDeny();
        headerPolicyCollection.AddXssProtectionBlock();
        headerPolicyCollection.AddContentTypeOptionsNoSniff();
        headerPolicyCollection.AddStrictTransportSecurityMaxAgeIncludeSubDomains();
        headerPolicyCollection.AddReferrerPolicyStrictOriginWhenCrossOrigin();
        headerPolicyCollection.RemoveServerHeader();

        headerPolicyCollection.AddContentSecurityPolicy(builder =>
        {
            builder.AddObjectSrc().Self();
            builder.AddFormAction().Self();
            builder.AddFrameAncestors().Self();
            builder.AddStyleSrc().Self();
            builder.AddScriptSrc().Self();
            builder.AddFontSrc().Self();
            builder.AddMediaSrc().Self();
            builder.AddConnectSrc().Self();
        });

        headerPolicyCollection.AddCrossOriginOpenerPolicy(builder => { builder.SameOrigin(); });

        headerPolicyCollection.AddCrossOriginEmbedderPolicy(builder => { builder.RequireCorp(); });

        headerPolicyCollection.AddCrossOriginResourcePolicy(builder => { builder.SameOrigin(); });

        headerPolicyCollection.AddPermissionsPolicy(builder =>
        {
            builder.AddAmbientLightSensor().None();
            builder.AddAccelerometer().None();
            builder.AddAutoplay().Self();
            builder.AddCamera().None();
            builder.AddEncryptedMedia().Self();
            builder.AddFullscreen().All();
            builder.AddGeolocation().None();
            builder.AddGyroscope().None();
            builder.AddMagnetometer().None();
            builder.AddMicrophone().None();
            builder.AddMidi().None();
            builder.AddPayment().None();
            builder.AddPictureInPicture().None();
            builder.AddSpeaker().None();
            builder.AddSyncXHR().None();
            builder.AddUsb().None();
            builder.AddVR().None();
        });

        return headerPolicyCollection;
    }
}
