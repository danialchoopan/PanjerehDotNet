# راهنمای پیاده‌سازی اپلیکیشن اندروید برای PanjerehDotNet

این سند به منظور راهنمایی توسعه‌دهندگان اندروید جهت اتصال به APIهای سامانه PanjerehDotNet و استفاده از قابلیت‌های پیشرفته آن تهیه شده است.

## ۱. مدیریت احراز هویت (JWT Bearer Token)

برای تعامل با بخش‌های محافظت شده API، باید توکن دریافت شده از مرحله تایید OTP را در هدر درخواست‌های خود قرار دهید.

### استفاده از Retrofit
توصیه می‌شود از یک `Interceptor` برای افزودن خودکار توکن به تمام درخواست‌ها استفاده کنید:

```kotlin
class AuthInterceptor(private val token: String) : Interceptor {
    override fun intercept(chain: Interceptor.Chain): Response {
        val request = chain.request().newBuilder()
            .addHeader("Authorization", "Bearer $token")
            .build()
        return chain.proceed(request)
    }
}
```

## ۲. کار با مکان (Location) و فیلترها

هنگام دریافت لیست آگهی‌ها یا ارسال آگهی جدید، مقادیر جغرافیایی به صورت زیر مدیریت می‌شوند:

- **دریافت:** فیلدهای `latitude` و `longitude` در پاسخ JSON آگهی‌ها موجود است.
- **ارسال:** مختصات را به صورت `Double` به همراه سایر فیلدها در قالب `multipart/form-data` ارسال کنید.
- **فیلتر:** برای فیلتر بر اساس منطقه، از پارامتر `query` یا `categoryId` در متد GET لیست آگهی‌ها استفاده کنید.

## ۳. پیاده‌سازی چت زنده با SignalR

برای برقراری ارتباط چت آنی در اندروید، از کتابخانه رسمی SignalR Java Client استفاده کنید.

### اتصال به Hub
```kotlin
val hubConnection = HubConnectionBuilder.create("http://YOUR_SERVER_IP/chatHub")
    .withAccessTokenProvider(Single.just(userToken))
    .build()

hubConnection.start().blockingAwait()
```

### دریافت پیام
```kotlin
hubConnection.on("ReceiveMessage", { senderId, adId, content, sentAt ->
    // نمایش پیام در رابط کاربری
}, Int::class.java, Int::class.java, String::class.java, String::class.java)
```

### ارسال پیام
```kotlin
hubConnection.send("SendMessage", receiverId, adId, "سلام، متن پیام")
```

## ۴. آپلود تصاویر آگهی

تصاویر باید به صورت `MultipartBody.Part` با استفاده از Retrofit ارسال شوند. سرور تصاویر را به صورت محلی ذخیره کرده و آدرس نسبی آن‌ها را در پایگاه داده ثبت می‌کند.
