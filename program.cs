using System;

class Agen
{
    public string id_agen;
    public string nama_agen;
    public int stok_agen;

    public Agen(string id, string nama, int stok)
    {
        id_agen = id;
        nama_agen = nama;
        stok_agen = stok;
    }

    public void TampilInfo()
    {
        Console.WriteLine("ID Agen      : " + id_agen);
        Console.WriteLine("Nama Agen    : " + nama_agen);
        Console.WriteLine("Stok Galon   : " + stok_agen);
    }
}


abstract class ProdukGalon
{
    private string _id_produk;
    private string _nama_produk;
    private int _stok_gudang;

    public string id_produk
    {
        get { return _id_produk; }
        set { _id_produk = value; }
    }

    public string nama_produk
    {
        get { return _nama_produk; }
        set { _nama_produk = value; }
    }

    public int stok_gudang
    {
        get { return _stok_gudang; }
        private set
        {
            if (value >= 0)
            {
                _stok_gudang = value;
            }
        }
    }

    public ProdukGalon(string id, string nama, int stok)
    {
        id_produk = id;
        nama_produk = nama;

        if (stok < 0)
        {
            stok = 0;
        }

        stok_gudang = stok;
    }

    public bool KurangiStok(int jumlah)
    {
        if (jumlah <= 0)
        {
            return false;
        }

        if (jumlah > stok_gudang)
        {
            return false;
        }

        stok_gudang = stok_gudang - jumlah;

        return true;
    }

    public abstract int HitungBiaya(int jumlah);
}


class GalonAkua : ProdukGalon
{
    public GalonAkua(string id, string nama, int stok)
        : base(id, nama, stok)
    {

    }

    public override int HitungBiaya(int jumlah)
    {
        return jumlah * 20000;
    }
}


class GalonBerkeran : ProdukGalon
{
    public string jenis_keran;

    public GalonBerkeran(string id, string nama, int stok, string keran)
        : base(id, nama, stok)
    {
        jenis_keran = keran;
    }

    public override int HitungBiaya(int jumlah)
    {
        return jumlah * 12000;
    }
}



interface DapatDidistribusikan
{
    void KirimPaket();
}


class DistribusiGalon : DapatDidistribusikan
{
    private Agen agen_tujuan;
    private ProdukGalon produk_dikirim;
    private int jumlah;

    public DistribusiGalon(Agen agen, ProdukGalon produk, int jml)
    {
        agen_tujuan = agen;
        produk_dikirim = produk;
        jumlah = jml;
    }

    public void KirimPaket()
    {
        if (agen_tujuan == null || produk_dikirim == null)
        {
            Console.WriteLine("Pengiriman gagal, data agen atau produk tidak ditemukan.");
            return;
        }

        if (jumlah <= 0)
        {
            Console.WriteLine("Pengiriman gagal, jumlah galon harus lebih dari 0.");
            return;
        }

        if (jumlah > produk_dikirim.stok_gudang)
        {
            Console.WriteLine("Pengiriman gagal, stok gudang tidak mencukupi.");
            Console.WriteLine("Stok tersedia : " + produk_dikirim.stok_gudang);
            return;
        }

        bool berhasil = produk_dikirim.KurangiStok(jumlah);

        if (berhasil == true)
        {
            agen_tujuan.stok_agen = agen_tujuan.stok_agen + jumlah;

            Console.WriteLine("Paket berhasil dikirimkan ke agen " + agen_tujuan.nama_agen);
            Console.WriteLine("Jumlah galon   : " + jumlah);
            Console.WriteLine("Produk         : " + produk_dikirim.nama_produk);
            Console.WriteLine("Sisa stok      : " + produk_dikirim.stok_gudang);
        }
        else
        {
            Console.WriteLine("Pengiriman gagal, terjadi kesalahan saat mengurangi stok.");
        }
    }
}


class Program
{
    static void Main(string[] args)
    {
        Agen agen_1 = new Agen("AG001", "Agen Jakarta", 10);
        Agen agen_2 = new Agen("AG002", "Agen Bandung", 5);


        agen_1.TampilInfo();
        agen_2.TampilInfo();


        ProdukGalon produk_1 = new GalonAkua("P001", "Air Galon Akua", 20);

        ProdukGalon produk_2 = new GalonBerkeran("P002", "Air Galon Berkeran", 15, "Keran Putar");


        Console.WriteLine("\nDATA PRODUK");
        Console.WriteLine("Produk 1 : " + produk_1.nama_produk);
        Console.WriteLine("Stok     : " + produk_1.stok_gudang);

        Console.WriteLine();

        Console.WriteLine("Produk 2 : " + produk_2.nama_produk);
        Console.WriteLine("Stok     : " + produk_2.stok_gudang);


        Console.WriteLine("\nPERHITUNGAN BIAYA");

        int total_akua = produk_1.HitungBiaya(3);
        int total_berkeran = produk_2.HitungBiaya(3);

        Console.WriteLine("Biaya 3 Galon Akua     : Rp" + total_akua);
        Console.WriteLine("Biaya 3 Galon Berkeran : Rp" + total_berkeran);


        Console.WriteLine("\nDISTRIBUSI");

        DistribusiGalon kirim_1 = new DistribusiGalon(agen_1, produk_1, 5);
        kirim_1.KirimPaket();

        Console.WriteLine();

        DistribusiGalon kirim_2 = new DistribusiGalon(agen_2, produk_2, 3);
        kirim_2.KirimPaket();
    }
}
