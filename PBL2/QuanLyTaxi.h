#include <iostream>
#include <vector>
#include <unordered_map>
#include <iomanip>
#include <string>
#include <fstream>
#include <sstream>
#include <windows.h>
#include <conio.h>
#include <algorithm>

#include "Taxi.h"
#include "TaiXe.h"
#include "ChuyenXe"

using namespace std;


class QuanLyTaxi {
private:
    vector<TaiXe> dsTaiXe;
    unordered_map<string, TaiXe*> taiXeByID;
    vector<Taxi> dsTaxi;
    vector<ChuyenXe> dsChuyenXe;
    unordered_map<string, pair<string, string>> users; 
    string currentRole; 


    void printHeader(const string& title);

    // Hien thi menu doc, tra ve index muc da chon, -1 neu ESC
    int showMenu(const string& title, const vector<string>& items, const string& hint= "Mui ten Len/Xuong de di chuyen, Enter de chon, ESC de quay lai. So 1-9 de chon nhanh.");

    void pause();

    // Đọc danh sách người dùng từ file users.txt
    void docUsers();

    // Ghi danh sách người dùng vào file users.txt
    void ghiUsers();

    // Đăng ký tài khoản
    void dangKy();

    // Đăng nhập
    bool dangNhap();

    // Đọc danh sách tài xế từ file drivers.txt
    void docTaiXe();

    // Ghi danh sách tài xế vào file drivers.txt
    void ghiTaiXe();

    // Đọc danh sách taxi từ file cars.txt
    void docTaxi();

    // Ghi danh sách taxi vào file cars.txt
    void ghiTaxi();
    // Đọc danh sách chuyến xe từ file customers.txt
    void docChuyenXe();

    // Ghi danh sách chuyến xe vào file customers.txt
    void ghiChuyenXe();
public:
    QuanLyTaxi();

    void themTaiXe();

    void hienThiTaiXe();

    void themTaxi();

    void ganTaiXeChoTaxi();

    void hienThiTaxi();

    void themChuyenXe();

    void hienThiChuyenXe();

    void timTaiXe();

	    // ===================== THỐNG KÊ DOANH THU =====================
    void thongKeTheoThoiGian();

    void thongKeTheoTaiXe();

    void thongKeTheoTaxi();

    void thongKeTongHop() ;

    void menu();
};

