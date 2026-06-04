# مستندات فنی REST API سامانه PanjerehDotNet

این سند شامل جزئیات فنی تمام نقاط انتهایی (Endpoints) طراحی شده در کنترلرهای سامانه است. تمام درخواست‌ها و پاسخ‌ها از فرمت JSON استفاده می‌کنند.

## احراز هویت (Auth)

### ارسال کد تایید (OTP)
- **آدرس:** `/api/Auth/send-otp`
- **متد:** `POST`
- **بدنه درخواست:**
  ```json
  "09121234567"
  ```
- **پاسخ موفق (200 OK):**
  ```json
  {
    "message": "OTP sent",
    "otp": "123456"
  }
  ```

### تایید کد و دریافت توکن
- **آدرس:** `/api/Auth/verify-otp`
- **متد:** `POST`
- **بدنه درخواست:**
  ```json
  {
    "phoneNumber": "09121234567",
    "otp": "123456"
  }
  ```
- **پاسخ موفق (200 OK):**
  ```json
  {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ..."
  }
  ```

---

## آگهی‌ها (Ads)

### دریافت لیست آگهی‌ها
- **آدرس:** `/api/Ads`
- **متد:** `GET`
- **پارامترها:** `page`, `pageSize`, `categoryId`, `query`
- **پاسخ موفق (200 OK):**
  ```json
  [
    {
      "id": 1,
      "title": "عنوان آگهی",
      "description": "توضیحات...",
      "price": 500000,
      "cityName": "تهران",
      "images": ["/uploads/img1.jpg"]
    }
  ]
  ```

### دریافت جزئیات یک آگهی
- **آدرس:** `/api/Ads/{id}`
- **متد:** `GET`
- **پاسخ موفق (200 OK):** شامل تمام فیلدهای آگهی به همراه مختصات جغرافیایی (Latitude/Longitude).

### ثبت آگهی جدید
- **آدرس:** `/api/Ads`
- **متد:** `POST`
- **هدر:** `Authorization: Bearer {token}`
- **نوع محتوا:** `multipart/form-data`
- **فیلدها:** `Title`, `Description`, `Price`, `CategoryId`, `City`, `District`, `Latitude`, `Longitude`, `Images` (فایل)

---

## چت (Chat)

### دریافت تاریخچه گفتگو
- **آدرس:** `/api/Chat/history`
- **متد:** `GET`
- **هدر:** `Authorization: Bearer {token}`
- **پارامترها:** `otherUserId`, `adId`

### دریافت لیست گفتگوها
- **آدرس:** `/api/Chat/conversations`
- **متد:** `GET`
- **هدر:** `Authorization: Bearer {token}`

---

## مدیریت (Admin)

### دریافت آمار کلی
- **آدرس:** `/api/Admin/stats`
- **متد:** `GET`
- **هدر:** `Authorization: Bearer {token}` (نقش Admin الزامی است)

### تایید آگهی
- **آدرس:** `/api/Admin/approve-ad/{id}`
- **متد:** `POST`
- **هدر:** `Authorization: Bearer {token}`
