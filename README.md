# DodLang

**DodLang** adalah bahasa pemrograman sederhana buatan sendiri yang mendukung pernyataan cetak, operasi aritmatika, dan variabel global. Bahasa ini dirancang untuk tujuan pembelajaran dan eksperimental.

---

## ✨ Fitur

- Menampilkan output ke terminal menggunakan `cetak`
- Mendukung operasi aritmatika dasar (`+`, `-`, `*`, `/`)
- Variabel global menggunakan perintah `simpan`
- Eksekusi program dari file berekstensi `.dod`

---

## 📚 Sintaks Dasar

| Sintaks         | Deskripsi                                      |
|------------------|-----------------------------------------------|
| `cetak`          | Menampilkan teks atau hasil ekspresi ke layar |
| `simpan`         | Menyimpan nilai ke dalam variabel global      |
| `+`              | Penjumlahan angka atau penggabungan string     |
| `;`              | Menandai akhir dari setiap pernyataan         |

---

## 📦 Contoh Program

### 1. Cetak dan Operasi Aritmatika

File: `index.dod`

```dod
cetak "Hello World!";
cetak 1+1;
cetak 1+5*3;
cetak "Hello " + "Doddy";
```

**Output:**

```
Hello World!
2
16
Hello Doddy
```

---

### 2. Variabel Global

```dod
simpan a=1;
simpan b=2;
cetak a+b;
```

**Output:**

```
3
```

---

## ▶️ Cara Menjalankan DodLang

Pastikan kamu sudah membangun executable bernama `dodLang.exe` atau `Interpreter.exe`.

### 1. Jalankan dari Terminal atau PowerShell

```bash
.\dodLang.exe dod index.dod
```

Atau:

```bash
.\Interpreter.exe dod index.dod
```

### 2. Contoh File `index.dod`

```dod
simpan a=10;
simpan b=5;
cetak "Hasil:";
cetak a + b;
```

### 3. Output

```
Hasil:
15
```

### 4. Tips

- File harus disimpan dengan ekstensi `.dod`
- Setiap pernyataan diakhiri dengan `;`
- Jalankan dari folder yang sama dengan file `.dod`

---

## 📁 Struktur Direktori

```
DodInterpreter/
├── dodLang.exe
├── Interpreter.exe
├── index.dod
└── ...
```

---

## 🚧 Keterbatasan Saat Ini

- Semua variabel bersifat global (belum ada scope)
- Belum mendukung fungsi, perulangan, atau percabangan (seperti `if`, `while`)
- Hanya mendukung integer dan string

---

## 🧑‍💻 Pengembang

Doddy Pranatha

Jika kamu tertarik berkontribusi, membuat fitur baru, atau ingin memperluas bahasa ini, silakan hubungi saya.

---

## 📜 Lisensi

Proyek ini bebas digunakan untuk tujuan pembelajaran dan penelitian.
