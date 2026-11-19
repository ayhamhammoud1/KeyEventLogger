# Tuş Günlüğü · Key Event Logger

YMH 219 Nesne Tabanlı Programlama dersi için geliştirilmiş bir Windows Forms uygulaması. Kullanıcı kaydı başlattığında, etkin uygulama penceresindeki tuş olayları saat ve tuş adıyla listelenir.

## Öğrenci bilgileri

Ad Soyad: Ayham Nawaf Hammoud 
Öğrenci numarası: 240541613

## Teknolojiler

C#, Windows Forms, .NET Framework 4.8 ve Visual Studio. Uygulama harici çalışma zamanı paketi kullanmaz.

## Kurulum ve çalıştırma

1. ZIP dosyasını bir klasöre çıkarın.
2. Windows'ta Visual Studio 2022 veya daha yeni bir sürümde **KeyEventLogger.sln** dosyasını açın. Kurulumda **.NET desktop development** ve **.NET Framework 4.8 targeting pack** bileşenlerini seçin.
3. Başlatmak için **F5** tuşuna basın.

## Kullanım ve temel özellikler

- **Başlat** ile kaydı açın. Pencere etkin durumdayken A, B ve Space tuşlarına basın. Her satırda milisaniyeli yerel saat ve tuş adı görünür.
- Başka bir pencereye geçin. Uygulama odağı bekler. Geri döndüğünüzde olay sayısı aynı kalır ve kayda devam edebilirsiniz.
- **Durdur** kaydı sonlandırır ve listeyi korur. **Başlat** aynı listeye yeni olaylar ekler.
- **.txt kaydet** ile bir konum seçin. Dosyada tarih, saat ve tuş adı UTF-8 biçiminde saklanır.
- **Temizle** listeyi ve sayacı sıfırlar. Daha önce kaydettiğiniz dosyalar korunur.

## Ekran görüntüleri

### Başlangıç

![Kayıt kapalı ve olay listesi boş](docs/baslangic.png)

### Etkin kayıt

![Kayıt açıkken yedi tuş olayı ve toplam olay sayısı](docs/kayit.png)

## Kodun yapısı

Program.cs uygulamayı başlatır. MainForm.cs düğmeleri, kayıt durumunu ve formun KeyDown olayını yönetir. KeyEntry.cs tek bir olayın zamanını ve tuş adını tutar, ekrandaki ve dosyadaki gösterimini üretir.

## Bilinen sınırlamalar ve geliştirme notları

Form yalnızca kendisine ulaşan klavye olaylarını işler. Windows'un yönettiği sistem kısayolları ve gezinme amacıyla kullanılan bazı tuşlar listeye düşmeyebilir. Tuş basılı tutulduğunda işletim sisteminin ürettiği tekrar olayları ayrı satırlarda görünür. Kayıtlar elle kaydedilene kadar bellekte tutulur.

Ekran görüntüleri ve işlev denemeleri Mono/WinForms üzerinde alınmıştır. Windows'ta yazı tipi ve sistem iletişim kutularının görünümü farklı olabilir.
