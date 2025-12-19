# Carton Caps – Referral Feature (Mock API)

## 📌 Overview

This project is a **Mock REST API** built with **.NET** as part of the *HatchWorks .NET Code Challenge*.  
The purpose of this service is to support the **Refer-a-Friend** feature for the Carton Caps mobile application, allowing existing users to invite friends using referral links and enabling the app to track referral activity.

The API serves as a **contract between backend and mobile frontend**, providing realistic mock data suitable for development and testing.

---

## 🎯 Challenge Context

Carton Caps is a mobile application that empowers consumers to raise money for schools while purchasing everyday products.  
This challenge focuses on designing and implementing the backend support for a new **Referral Feature**, powered by **deferred deep links**, which allows the app to present a customized onboarding experience after installation.

### Assumptions (per challenge instructions)

- User authentication already exists  
- Users already have a referral code  
- New user registration already exists  
- Referral redemption is handled during registration  
- A third-party vendor is used for deferred deep link support  
- This API **does not send SMS or emails**, it only supports referral logic and tracking  

---

## 🧩 Features Implemented

- Generate referral links for sharing  
- Create referral invitations (intent tracking)  
- Retrieve referral invitations by referrer  
- Resolve referral codes after app installation  
- Track referral completion  
- Firebase-backed mock persistence  
- Unit tests covering business logic and controllers  

---

## 🚀 Running the Project Locally

### Prerequisites

- .NET SDK 8 or later  
- macOS, Windows, or Linux  

Verify your installation:

```bash
dotnet --version
```

---

### Run the API

From the root of the repository:

```bash
dotnet restore
dotnet build
dotnet run --project src/Api
```

The API will be available at:

```text
http://localhost:5145
```

Alternatively, you can run using the default profile:

```bash
dotnet run --project src/Api --launch-profile https
```

This will start the API on both `https://localhost:7066` and `http://localhost:5145`.

---

## 📚 API Documentation

Once the API is running, you can explore and test all endpoints using any of the following documentation interfaces:

- **Swagger UI**  
  http://localhost:5145/swagger/index.html  

- **ReDoc (API Documentation)**  
  http://localhost:5145/redoc/index.html  

- **Scalar API Reference**  
  http://localhost:5145/scalar/v1  

These interfaces provide request/response schemas, example payloads, and endpoint descriptions.

### Available Endpoints

All endpoints are prefixed with `/api/referrals`:

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/referrals` | Get all referrals |
| `POST` | `/api/referrals` | Create a new referral invitation |
| `GET` | `/api/referrals/{userId}` | Get referrals created by a specific user (by referrer User ID) |
| `GET` | `/api/referrals/resolve/{referralId}` | Resolve/complete a referral code |

#### Request Examples

**Create Referral Invitation** (`POST /api/referrals`)

```json
{
  "channel": "sms",
  "recipientName": "John Doe",
  "recipientContact": "+1234567890",
  "referralCode": "REF123ABC",
  "referrerUserId": "user-id-here"
}
```

**Get User Referrals** (`GET /api/referrals/{userId}`)

Returns all referral invitations created by the specified user with their status (Pending or Completed).

**Resolve Referral** (`GET /api/referrals/resolve/{referralId}`)

Marks a referral code as completed. Can only be completed once; subsequent attempts will return an error.

---

## 🧪 Running Tests

Unit tests are included to validate the business logic and controller behavior.

Run all tests with:

```bash
dotnet test
```

---

## 🗄️ Database Configuration (Firebase)

This project uses **Firebase Firestore** as the database for mock persistence.

### Firebase Database Setup

The Firebase database is configured in the `DependencyInjection.cs` file within the Persistence project:

- **Database Project ID**: `cartoncaps-ca15c` (hardcoded)
- **Credentials**: Loaded from the environment variable `GOOGLE_APPLICATION_CREDENTIALS`

The credentials file path is specified in `appsettings.json`:

```json
"FirebaseDataBase": "cartoncaps-ca15c-firebase-adminsdk-fbsvc-0b5b1aec1d.json"
```

This value is read from configuration and set as the `GOOGLE_APPLICATION_CREDENTIALS` environment variable at runtime, allowing the Firebase SDK to authenticate.

### Changing Firebase Credentials or Database

To use a different Firebase project:

1. **Download new credentials**: From your Firebase project settings, download the service account JSON file
2. **Replace the credentials file**:
   - Remove or rename: `src/Api/cartoncaps-ca15c-firebase-adminsdk-fbsvc-0b5b1aec1d.json`
   - Place the new credentials file in the `src/Api` folder
3. **Update `appsettings.json`**:
   ```json
   "FirebaseDataBase": "<new-credentials-filename>.json"
   ```
4. **Update the database ID** in `src/Persistence/DependencyInjection.cs`:
   ```csharp
   services.AddSingleton(s => FirestoreDb.Create("<your-database-project-id>"));
   ```
5. **Restart the API**

No other code changes are required to switch databases.

---

## 🔐 Security & Abuse Considerations

Although this is a mock service, the design accounts for future scalability and abuse prevention:

- Referral codes can only be completed once  
- Completed referrals cannot be reused  
- Business rules prevent duplicate referral rewards  
- Architecture supports future rate limiting and validation strategies  

---

## 🧠 Design Notes

- Clear separation between Controllers, Services, and Repositories  
- Business logic fully unit-tested  
- Frontend-friendly API design  
- Privacy-respecting: no access to SMS or email content  
- Fully compatible with macOS (as required by the challenge)  

---

## ✅ Summary

This project demonstrates:

- Clean and maintainable .NET API design  
- Proper handling of referral-based workflows  
- Realistic mock service suitable for frontend development  
- Strong testing practices  
- Alignment with the HatchWorks engineering challenge requirements  
