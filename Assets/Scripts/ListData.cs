using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ListData
{
    public List<DashboardData> dashboard_data;
    public List<Inisial> inisial_peneliti;
    public List<Peneliti> nama_peneliti;
    public List<Fakultas> fakultas_peneliti;
    public List<Departemen> departemen_peneliti;
    public List<Gelar> gelar_peneliti;
    public List<DetailPeneliti> detail_peneliti;
    public List<HasilPublikasiITS> hasil_publikasi;
    public List<KataKunci> fakultas_publikasi;
}

[System.Serializable]
public class DashboardData
{
    public List<HasilPublikasiITS> hasil_publikasi;
}

[System.Serializable]
public class HasilPublikasiITS
{
    public int journals;
    public int conferences;
    public int books;
    public int thesis;
    public int paten;
    public int research;
}

[System.Serializable]
public class DetailPeneliti
{
    public string nama;
    public string tanggal_lahir;
    public string fakultas;
    public string departemen;
    public int jurnal;
    public int konferensi;
    public int buku;
    public int tesis;
    public int paten;
    public int penelitian;
}

[System.Serializable]
public class Inisial
{
    public string inisial;
    public int total;
}

[System.Serializable]
public class Peneliti
{
    public string nama_departemen;
    public string kode_dosen;
    public string nama;
    public int jumlah;
}

[System.Serializable]
public class Fakultas
{
    public int kode_fakultas;
    public string nama_fakultas;
    public int jumlah;  
}

[System.Serializable]
public class Departemen
{
    public int kode_fakultas;
    public string nama_fakultas;
    public int kode_departemen;
    public string nama_departemen;
    public int jumlah;
}

[System.Serializable]
public class Gelar
{
    public string gelar;
    public int jumlah;
}

[System.Serializable]
public class KataKunci
{
    public int kode_fakultas;
    public string nama_fakultas;
    public string kata_kunci;
    public string idf;
    public string df;
}