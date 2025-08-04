# EventManagementBackend
EventManagementBackend, kullanıcıların etkinlikleri listeleyebildiği, kayıt olabildiği ve diğer birçok özelliği yönetebildiği bir mikroservis tabanlı backend uygulamasıdır. Proje .NET 8, MediatR ve Entity Framework Core kullanılarak geliştirilmiştir. 

## İçerik
-[Genel Bilgi](#genel-bilgi)
-[Kullanılan Teknolojiler](#kullanılan-teknolojiler)
-[Katmanlar](#katmanlar)
-[API Uç Noktalar](#api-uç-noktaları)

## Genel Bilgi
Bu proje iki temel servisten oluşur:
- 'UserService' : Kullanıcı kayıt, giriş, doğrulama gibi işlemlerin yer aldığı servistir.
- 'EventService' : Event oluşturma, güncelleme, silme gibi işlemlerin gerçekleştirildiği servistir.

## Kullanılan Teknolojiler
- ASP.NET Core Web API
- CQRS Pattern + MediatR
- Entity Framework Core
- SQL Server
- ClosedXML (Excel export)
- FluentValidation
- IMemoryCache
- Serilog
- Swagger UI
- Docker

## Katmanlar
EventManagementBackend/
│
├── EventService/
│ ├── Controllers
│ ├── Handlers
│ ├── Models
│ └── Repositories
│
├── UserService/
│ ├── Controllers
│ ├── Handlers
│ ├── Models
│ └── Repositories
│
├── EventManagementBackend.sln
├── README.md
└── .gitignore

## API Uç Noktaları
API Uç Noktaları

    UserService
        POST    /api/User/register                  → Yeni kullanıcı kaydı
        POST    /api/User/login                     → Kullanıcının sisteme girişi
        GET     /api/User/{id}                      → Kullanıcı bilgisi
        GET     /api/User/by-mail?mail={email}      → Mail ile kullanıcı arama
        DELETE  /api/User/{id}                      → Kullanıcı sil

    EventService
        POST    /api/Event/create                   → Yeni etkinlik oluşturma
        POST    /api/Event/register-to-event        → Kullanıcıyı etkinliğe kaydet
        POST    /api/Event/export-excel             → Etkinlikleri Excel olarak dışa aktar
        POST    /api/Event/register-to-event        → Kullanıcının etkinliğe katılması
        GET     /api/Event/{id}                     → Etkinlik getir
        GET	    /api/Event/all                      → Tüm etkinlikleri getir
        GET	    /api/Event/current                  → Gelecek etkinlikleri getir
        GET	    /api/Event/paged?page=1&pageSize=10 → Sayfalanmış liste
        GET	    /api/Event/search?keyword=konser    → Anahtar kelimeyle arama
        DELETE  /api/Event/delete/{id}              → Etkinliği silme
