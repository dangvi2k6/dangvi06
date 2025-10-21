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
using namespace std;

void setColor(int color) {
    SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), color);
}

class TaiXe {
public:
    string ID, tenTaiXe, ngaySinh, sdt, bangLai;
    bool trangThai;
    TaiXe(const string& id = "", const string& ten = "", const string& ns = "",
          const string& dt = "", const string& bl = "", bool tt = true)
        : ID(id), tenTaiXe(ten), ngaySinh(ns), sdt(dt), bangLai(bl), trangThai(tt) {}
};

class Taxi {
public:
    string bienSo;
    int sucChua;
    vector<string> dsTaiXe;
    Taxi(const string& bs = "", int sc = 0) : bienSo(bs), sucChua(sc) {}
};

class ChuyenXe {
public:
    string maChuyen, tenTaiXe, tenKhach, sdtKhach, bienSoTaxi, thoiDiem;
    double khoangCach, thoiGian, cuocPhi;
    ChuyenXe(const string& mc = "", const string& tk = "", const string& ttx = "",
             const string& sdt = "", const string& bs = "", const string& td = "",
             double kc = 0.0, double tg = 0.0, double cp = 0.0)
        : maChuyen(mc), tenKhach(tk), tenTaiXe(ttx), sdtKhach(sdt), bienSoTaxi(bs),
          thoiDiem(td), khoangCach(kc), thoiGian(tg), cuocPhi(cp) {}
};

class QuanLyTaxi {
private:
    vector<TaiXe> dsTaiXe;
    unordered_map<string, TaiXe*> taiXeByID;
    vector<Taxi> dsTaxi;
    vector<ChuyenXe> dsChuyenXe;
    unordered_map<string, pair<string, string>> users; // Lưu trữ username, password và role
    string currentRole; // Lưu vai trò hiện tại (admin hoặc user)

    //======Helps cho UI menu======
    void printHeader(const string& title){
        setColor(10);
        cout<<"=== "<<title<<" ===\n";
        setColor(7);
    }

    // Hien thi menu doc, tra ve index muc da chon, -1 neu ESC
    int showMenu(const string& title, const vector<string>& items, const string& hint= "Mui ten Len/Xuong de di chuyen, Enter de chon, ESC de quay lai. So 1-9 de chon nhanh.") {
        int selected=0;
        while(true) {
            system("cls");
            printHeader(title);
            
            for(int i=0; i< (int)items.size(); ++i){
                if(i==selected) {
                    setColor(11);
                    cout<<">> "<<items[i]<<"\n";
                    setColor(7);
                }
                else{
                    cout<<"  "<<items[i]<<"\n";
                }
            }
            cout<<"\nHuong dan: "<<hint<<"\n";

            int ch=_getch();
            //Phim mui ten la chuoi 224 roi den ma tiep theo
            if(ch==224||ch==0){
                int code=_getch();
                if(code==72){ // Up
                    selected=(selected-1+(int)items.size())%(int)items.size();
                }
                else if(code==80){
                    selected=(selected+1)%(int)items.size();
                }
            }
            else if(ch==13){ //Enter
                return selected;
            }
            else if(ch==27){ //ESC
                return -1;
            }
            else if(ch>='1'&&ch<='9'){
                int idx=ch-'1';
                if(idx>=0 && idx<(int)items.size()) return idx;
            }
        }
    }
    void pause(){
        cout<<"\nNhan phim bat ky de tiep tuc...";
        _getch();
    }   

    // Đọc danh sách người dùng từ file users.txt
    void docUsers() {
        ifstream file("users1.txt");
        if (!file.is_open()) return;

        string line;
        while (getline(file, line)) {
            stringstream ss(line);
            string username, password, role;
            getline(ss, username, '|');
            getline(ss, password, '|');
            getline(ss, role);
            users[username] = {password, role};
        }
        file.close();
    }

    // Ghi danh sách người dùng vào file users.txt
    void ghiUsers() {
        ofstream file("users1.txt");
        for (const auto& user : users) {
            file << user.first << "|" << user.second.first << "|" << user.second.second << endl;
        }
        file.close();
    }

    // Đăng ký tài khoản
    void dangKy() {
        system("cls");
        printHeader("DANG KY TAI KHOAN");
        string username, password, role;
        cout << "Nhap ten dang nhap: ";
        cin >> username;

        if (users.find(username) != users.end()) {
            setColor(12); cout << "Ten dang nhap da ton tai!\n"; setColor(7);
            pause();
            return;
        }

        cout << "Nhap mat khau: ";
        char ch;
        password = "";
        while ((ch = _getch()) != 13) { // Enter key
            if (ch == 8 && !password.empty()) { // Backspace
                password.pop_back();
                cout << "\b \b";
            } else if (ch >= 32 && ch <= 126) {
                password += ch;
                cout << "*";
            }
        }
        cout << endl;

        int choice = showMenu("CHON VAI TRO", {"Admin", "User"});
        if (choice == -1) {
            setColor(12); cout << "Huy dang ky!\n"; setColor(7);
            pause();
            return;
        }
        role = (choice == 0) ? "admin" : "user";

        users[username] = {password, role};
        ghiUsers();
        setColor(10); cout << "Dang ky tai khoan thanh cong!\n"; setColor(7);
        pause();
    }

    // Đăng nhập
    bool dangNhap() {
        while (true) {
            int choice = showMenu("DANG NHAP HE THONG", {"1. Admin", "2. User", "3. Dang ky", "4. Thoat"});
            if (choice == -1 || choice == 3) return false;
            if (choice == 2) {
                dangKy();
                continue; // Quay lại menu đăng nhập sau khi đăng ký
            }

            string selectedRole = (choice == 0) ? "admin" : "user";
            string username, password;
      
            int attempts = 3;

            while (attempts > 0) {
                system("cls");
                printHeader(string("DANG NHAP ") + (selectedRole == "admin" ? "ADMIN" : "USER"));
                cout << "Nhap ten dang nhap: ";
                cin >> username;
                cout << "Nhap mat khau: ";
                char ch;
                password = "";
                while ((ch = _getch()) != 13) { // Enter key
                    if (ch == 8 && !password.empty()) { // Backspace
                        password.pop_back();
                        cout << "\b \b";
                    } else if (ch >= 32 && ch <= 126) {
                        password += ch;
                        cout << "*";
                    }
                }
                cout << endl;

                auto it = users.find(username);
                if (it != users.end() && it->second.first == password && it->second.second == selectedRole) {
                    currentRole = selectedRole;
                    setColor(10); cout << "Dang nhap thanh cong!\n"; setColor(7);
                    pause();
                    return true;
                } else {
                    attempts--;
                    setColor(12); cout << "Ten dang nhap hoac mat khau sai! Con " << attempts << " lan thu.\n"; setColor(7);
                    pause();
                }
            }
            setColor(12); cout << "Het luot dang nhap. Quay lai menu chinh.\n"; setColor(7);
            pause();
        }
        return false;
    }

    // Đọc danh sách tài xế từ file drivers.txt
    void docTaiXe() {
        ifstream file("drivers.txt");
        if (!file.is_open()) return;

        string line;
        while (getline(file, line)) {
            stringstream ss(line);
            string id, ten, ngaySinh, sdt, bangLai;
            bool trangThai;
            string trangThaiStr;
            getline(ss, id, '|');
            getline(ss, ten, '|');
            getline(ss, ngaySinh, '|');
            getline(ss, sdt, '|');
            getline(ss, bangLai, '|');
            getline(ss, trangThaiStr);
            trangThai = (trangThaiStr == "1");

            dsTaiXe.emplace_back(id, ten, ngaySinh, sdt, bangLai, trangThai);
            taiXeByID[id] = &dsTaiXe.back();
        }
        file.close();
    }

    // Ghi danh sách tài xế vào file drivers.txt
    void ghiTaiXe() {
        ofstream file("drivers.txt");
        for (const auto& tx : dsTaiXe) {
            file << tx.ID << "|" << tx.tenTaiXe << "|" << tx.ngaySinh << "|"
                 << tx.sdt << "|" << tx.bangLai << "|" << (tx.trangThai ? "1" : "0") << endl;
        }
        file.close();
    }

    // Đọc danh sách taxi từ file cars.txt
    void docTaxi() {
        ifstream file("cars.txt");
        if (!file.is_open()) return;

        string line;
        while (getline(file, line)) {
            stringstream ss(line);
            string bienSo, dsTaiXeStr;
            int sucChua;
            getline(ss, bienSo, '|');
            ss >> sucChua;
            ss.ignore(1); // Bỏ qua dấu |
            getline(ss, dsTaiXeStr);

            Taxi taxi(bienSo, sucChua);
            if (!dsTaiXeStr.empty()) {
                stringstream ssTaiXe(dsTaiXeStr);
                string id;
                while (getline(ssTaiXe, id, ',')) {
                    if (taiXeByID.find(id) != taiXeByID.end()) {
                        taxi.dsTaiXe.push_back(id);
                    }
                }
            }
            dsTaxi.push_back(taxi);
        }
        file.close();
    }

    // Ghi danh sách taxi vào file cars.txt
    void ghiTaxi() {
        ofstream file("cars.txt");
        for (const auto& tx : dsTaxi) {
            file << tx.bienSo << "|" << tx.sucChua << "|";
            for (size_t i = 0; i < tx.dsTaiXe.size(); ++i) {
                file << tx.dsTaiXe[i];
                if (i < tx.dsTaiXe.size() - 1) file << ",";
            }
            file << endl;
        }
        file.close();
    }

    // Đọc danh sách chuyến xe từ file customers.txt
    void docChuyenXe() {
        ifstream file("customers.txt");
        if (!file.is_open()) return;

        string line;
        while (getline(file, line)) {
            stringstream ss(line);
            string maChuyen, tenKhach, tenTaiXe, sdtKhach, bienSoTaxi, thoiDiem;
            double khoangCach, thoiGian, cuocPhi;
            getline(ss, maChuyen, '|');
            getline(ss, tenKhach, '|');
            getline(ss, tenTaiXe, '|');
            getline(ss, sdtKhach, '|');
            getline(ss, bienSoTaxi, '|');
            getline(ss, thoiDiem, '|');
            ss >> khoangCach;
            ss.ignore(1);
            ss >> thoiGian;
            ss.ignore(1);
            ss >> cuocPhi;

            dsChuyenXe.emplace_back(maChuyen, tenKhach, tenTaiXe, sdtKhach, bienSoTaxi, thoiDiem, khoangCach, thoiGian, cuocPhi);
        }
        file.close();
    }

    // Ghi danh sách chuyến xe vào file customers.txt
    void ghiChuyenXe() {
        ofstream file("customers.txt");
        for (const auto& cx : dsChuyenXe) {
            file << cx.maChuyen << "|" << cx.tenKhach << "|" << cx.tenTaiXe << "|"
                 << cx.sdtKhach << "|" << cx.bienSoTaxi << "|" << cx.thoiDiem << "|"
                 << cx.khoangCach << "|" << cx.thoiGian << "|" << cx.cuocPhi << endl;
        }
        file.close();
    }

public:
    QuanLyTaxi() {
        docUsers();
        docTaiXe();
        docTaxi();
        docChuyenXe();
    }

    void themTaiXe() {
        system("cls");
        setColor(10); cout << "=== Them Tai Xe ===\n"; setColor(7);
        string id, ten, ngaySinh, sdt, bangLai;
        bool trangThai;
        cout << "Nhap ma tai xe: "; cin >> id;
        cin.ignore();
        cout << "Nhap ten: "; getline(cin, ten);
        cout << "Nhap ngay sinh: "; getline(cin, ngaySinh);
        cout << "Nhap so dien thoai: "; getline(cin, sdt);
        cout << "Nhap bang lai: "; getline(cin, bangLai);
        cout << "Nhap trang thai (1=ranh, 0=ban): "; cin >> trangThai;

        if (taiXeByID.find(id) != taiXeByID.end()) {
            setColor(12); cout << "ID tai xe da ton tai!\n"; setColor(7);
            cout << "\nNhan phim bat ky de tiep tuc..."; _getch();
            return;
        }

        dsTaiXe.emplace_back(id, ten, ngaySinh, sdt, bangLai, trangThai);
        taiXeByID[id] = &dsTaiXe.back();
        ghiTaiXe();
        setColor(10); cout << "Them tai xe thanh cong!\n"; setColor(7);
        cout << "\nNhan phim bat ky de tiep tuc..."; _getch();
    }

    void hienThiTaiXe() {
        system("cls");
        setColor(10); cout << "=== Danh Sach Tai Xe ===\n"; setColor(7);
        cout << left << setw(10) << "Ma" << setw(20) << "Ten" 
             << setw(15) << "Ngay sinh" << setw(15) << "SDT"
             << setw(15) << "Bang lai" << setw(10) << "Trang thai" << endl;
        cout << string(100, '-') << endl;
        for (const auto& tx : dsTaiXe) {
            cout << left << setw(10) << tx.ID << setw(20) << tx.tenTaiXe
                 << setw(15) << tx.ngaySinh << setw(15) << tx.sdt
                 << setw(15) << tx.bangLai << setw(10) << (tx.trangThai ? "Ranh" : "Ban") << endl;
        }
        cout << "\nNhan phim bat ky de tiep tuc..."; _getch();
    }

    void themTaxi() {
        system("cls");
        setColor(10); cout << "=== Them Taxi ===\n"; setColor(7);
        string bienSo;
        int sucChua;
        cout << "Nhap bien so: "; cin >> bienSo;
        cout << "Nhap suc chua: "; cin >> sucChua;
        dsTaxi.emplace_back(bienSo, sucChua);
        ghiTaxi();
        setColor(10); cout << "Them taxi thanh cong!\n"; setColor(7);
        cout << "\nNhan phim bat ky de tiep tuc..."; _getch();
    }

    void ganTaiXeChoTaxi() {
        system("cls");
        setColor(10); cout << "=== Gan Tai Xe Cho Taxi ===\n"; setColor(7);
        string bienSo, idTaiXe;
        cout << "Nhap bien so taxi: "; cin >> bienSo;
        cin.ignore();
        cout << "Nhap ID tai xe: "; getline(cin, idTaiXe);

        auto itTaxi = find_if(dsTaxi.begin(), dsTaxi.end(),
            [&bienSo](const Taxi& tx) { return tx.bienSo == bienSo; });
        if (itTaxi == dsTaxi.end()) {
            setColor(12); cout << "Taxi khong ton tai!\n"; setColor(7);
            cout << "\nNhan phim bat ky de tiep tuc..."; _getch();
            return;
        }

        if (taiXeByID.find(idTaiXe) == taiXeByID.end()) {
            setColor(12); cout << "Tai xe khong ton tai!\n"; setColor(7);
            cout << "\nNhan phim bat ky de tiep tuc..."; _getch();
            return;
        }

        itTaxi->dsTaiXe.push_back(idTaiXe);
        ghiTaxi();
        setColor(10); cout << "Gan tai xe thanh cong!\n"; setColor(7);
        cout << "\nNhan phim bat ky de tiep tuc..."; _getch();
    }

    void hienThiTaxi() {
        system("cls");
        setColor(10); cout << "=== Danh Sach Taxi ===\n"; setColor(7);
        cout << left << setw(15) << "Bien so" << setw(10) << "Suc chua" << setw(30) << "Danh sach tai xe" << endl;
        cout << string(100, '-') << endl;
        for (const auto& tx : dsTaxi) {
            cout << left << setw(15) << tx.bienSo << setw(10) << tx.sucChua;
            if (tx.dsTaiXe.empty()) {
                cout << setw(30) << "Chua co";
            } else {
                string dsTenTaiXe;
                for (const auto& id : tx.dsTaiXe) {
                    auto it = taiXeByID.find(id);
                    if (it != taiXeByID.end()) {
                        dsTenTaiXe += it->second->tenTaiXe + ", ";
                    }
                }
                if (!dsTenTaiXe.empty()) dsTenTaiXe.erase(dsTenTaiXe.size() - 2);
                cout << setw(30) << dsTenTaiXe;
            }
            cout << endl;
        }
        cout << "\nNhan phim bat ky de tiep tuc..."; _getch();
    }

    void themChuyenXe() {
        system("cls");
        setColor(10); cout << "=== Them Chuyen Xe ===\n"; setColor(7);
        string maChuyen, tenKhach, tenTaiXe, sdtKhach, bienSoTaxi, thoiDiem;
        double khoangCach, thoiGian, cuocPhi;
        cout << "Nhap ma chuyen: "; cin >> maChuyen;
        cin.ignore();
        cout << "Nhap ten khach: "; getline(cin, tenKhach);
        cout << "Nhap ten tai xe: "; getline(cin, tenTaiXe);
        cout << "Nhap SDT khach: "; getline(cin, sdtKhach);
        cout << "Nhap bien so taxi: "; getline(cin, bienSoTaxi);

        auto itTaxi = find_if(dsTaxi.begin(), dsTaxi.end(),
            [&bienSoTaxi](const Taxi& tx) { return tx.bienSo == bienSoTaxi; });
        if (itTaxi == dsTaxi.end()) {
            setColor(12); cout << "Bien so taxi khong ton tai!\n"; setColor(7);
            cout << "\nNhan phim bat ky de tiep tuc..."; _getch();
            return;
        }

        auto itTaiXe = find_if(dsTaiXe.begin(), dsTaiXe.end(),
            [&tenTaiXe](const TaiXe& tx) { return tx.tenTaiXe == tenTaiXe && tx.trangThai; });
        if (itTaiXe == dsTaiXe.end()) {
            setColor(12); cout << "Tai xe khong ton tai hoac dang ban!\n"; setColor(7);
            cout << "\nNhan phim bat ky de tiep tuc..."; _getch();
            return;
        }

        bool taiXeHopLe = find(itTaxi->dsTaiXe.begin(), itTaxi->dsTaiXe.end(), itTaiXe->ID) != itTaxi->dsTaiXe.end();
        if (!taiXeHopLe) {
            setColor(12); cout << "Tai xe khong duoc gan cho taxi nay!\n"; setColor(7);
            cout << "\nNhan phim bat ky de tiep tuc..."; _getch();
            return;
        }

        cout << "Nhap thoi diem: "; getline(cin, thoiDiem);
        cout << "Nhap khoang cach (km): "; cin >> khoangCach;
        cout << "Nhap thoi gian (gio): "; cin >> thoiGian;
        cout << "Nhap cuoc phi: "; cin >> cuocPhi;

        dsChuyenXe.emplace_back(maChuyen, tenKhach, tenTaiXe, sdtKhach, bienSoTaxi, thoiDiem, khoangCach, thoiGian, cuocPhi);
        itTaiXe->trangThai = false;
        ghiChuyenXe();
        ghiTaiXe();
        setColor(10); cout << "Them chuyen xe thanh cong!\n"; setColor(7);
        cout << "\nNhan phim bat ky de tiep tuc..."; _getch();
    }

    void hienThiChuyenXe() {
        system("cls");
        setColor(10); cout << "=== Danh Sach Chuyen Xe ===\n"; setColor(7);
        cout << left << setw(10) << "Ma chuyen" << setw(20) << "Ten khach" << setw(20) << "Ten tai xe"
             << setw(15) << "SDT khach" << setw(15) << "Bien so" << setw(15) << "Thoi diem"
             << setw(12) << "Khoang cach" << setw(12) << "Thoi gian" << setw(12) << "Cuoc phi" << endl;
        cout << string(120, '-') << endl;
        for (const auto& cx : dsChuyenXe) {
            cout << left << setw(10) << cx.maChuyen << setw(20) << cx.tenKhach << setw(20) << cx.tenTaiXe
                 << setw(15) << cx.sdtKhach << setw(15) << cx.bienSoTaxi << setw(15) << cx.thoiDiem
                 << setw(12) << cx.khoangCach << setw(12) << cx.thoiGian << setw(12) << cx.cuocPhi << endl;
        }
        cout << "\nNhan phim bat ky de tiep tuc..."; _getch();
    }

    void timTaiXe() {
        system("cls");
        setColor(10); cout << "=== Tim Kiem Tai Xe ===\n"; setColor(7);
        string input;
        char ch;

        cout << "Nhap ID hoac ten tai xe (nhan ESC de thoat): ";

        while (true) {
            if (_kbhit()) {
                ch = _getch();
                if (ch == 27) break;
                if (ch == 8 && !input.empty()) {
                    input.pop_back();
                } else if (ch >= 32 && ch <= 126) {
                    input += ch;
                }

                system("cls");
                setColor(10); cout << "=== Tim Kiem Tai Xe ===\n"; setColor(7);
                cout << "Nhap ID hoac ten tai xe (nhan ESC de thoat): " << input << endl;
                cout << string(100, '-') << endl;

                vector<TaiXe*> ketQua;
                for (const auto& tx : dsTaiXe) {
                    if (tx.ID.find(input) == 0 || tx.tenTaiXe.find(input) == 0) {
                        ketQua.push_back(const_cast<TaiXe*>(&tx));
                    }
                }

                if (!ketQua.empty()) {
                    cout << left << setw(10) << "Ma" << setw(20) << "Ten"
                         << setw(15) << "Ngay sinh" << setw(15) << "SDT"
                         << setw(15) << "Bang lai" << setw(10) << "Trang thai" << endl;
                    cout << string(100, '-') << endl;
                    for (const auto& tx : ketQua) {
                        cout << left << setw(10) << tx->ID << setw(20) << tx->tenTaiXe
                             << setw(15) << tx->ngaySinh << setw(15) << tx->sdt
                             << setw(15) << tx->bangLai << setw(10) << (tx->trangThai ? "Ranh" : "Ban") << endl;
                    }
                } else {
                    setColor(12); cout << "Khong tim thay tai xe phu hop!\n"; setColor(7);
                }
            }
        }

        cout << "\nNhan phim bat ky de tiep tuc..."; _getch();
    }

	    // ===================== THỐNG KÊ DOANH THU =====================
    void thongKeTheoThoiGian() {
        system("cls");
        setColor(10); cout << "=== Thong ke doanh thu theo thoi gian ===\n"; setColor(7);
        double tongNgay = 0, tongThang = 0, tongQuy = 0;
        string ngay, thang, quy;

        cout << "Nhap ngay (vd: 17-10-2025): ";
        cin >> ngay;
        cout << "Nhap thang (vd: 10-2025): ";
        cin >> thang;
        cout << "Nhap quy (vd: Q4-2025): ";
        cin >> quy;

        for (const auto& cx : dsChuyenXe) {
            if (cx.thoiDiem.find(ngay) != string::npos) tongNgay += cx.cuocPhi;
            if (cx.thoiDiem.find(thang) != string::npos) tongThang += cx.cuocPhi;
            if (cx.thoiDiem.find(quy) != string::npos) tongQuy += cx.cuocPhi;
        }

        cout << "\nDoanh thu theo ngay " << ngay << ": " << tongNgay << " VND\n";
        cout << "Doanh thu theo thang " << thang << ": " << tongThang << " VND\n";
        cout << "Doanh thu theo quy " << quy << ": " << tongQuy << " VND\n";
        pause();
    }

    void thongKeTheoTaiXe() {
        system("cls");
        setColor(10); cout << "=== Thong ke theo tai xe ===\n"; setColor(7);

        struct Info {
            int soChuyen = 0;
            double tongCuoc = 0;
            double tongKhoangCach = 0;
        };
        unordered_map<string, Info> thongKe;

        for (const auto& cx : dsChuyenXe) {
            thongKe[cx.tenTaiXe].soChuyen++;
            thongKe[cx.tenTaiXe].tongCuoc += cx.cuocPhi;
            thongKe[cx.tenTaiXe].tongKhoangCach += cx.khoangCach;
        }

        cout << left << setw(20) << "Ten tai xe"
             << setw(15) << "So chuyen"
             << setw(20) << "Doanh thu (VND)"
             << setw(20) << "Quang duong TB (km)" << endl;
        cout << string(75, '-') << endl;

        for (auto& [ten, val] : thongKe) {
            double tb = (val.soChuyen > 0) ? val.tongKhoangCach / val.soChuyen : 0;
            cout << left << setw(20) << ten
                 << setw(15) << val.soChuyen
                 << setw(20) << val.tongCuoc
                 << setw(20) << fixed << setprecision(2) << tb << endl;
        }
        pause();
    }

    void thongKeTheoTaxi() {
        system("cls");
        setColor(10); cout << "=== Thong ke theo taxi ===\n"; setColor(7);

        struct Info {
            int soChuyen = 0;
            double tongCuoc = 0;
        };
        unordered_map<string, Info> thongKe;

        for (const auto& cx : dsChuyenXe) {
            thongKe[cx.bienSoTaxi].soChuyen++;
            thongKe[cx.bienSoTaxi].tongCuoc += cx.cuocPhi;
        }

        cout << left << setw(15) << "Bien so"
             << setw(15) << "So chuyen"
             << setw(20) << "Doanh thu (VND)" << endl;
        cout << string(50, '-') << endl;

        for (auto& [bs, val] : thongKe) {
            cout << left << setw(15) << bs
                 << setw(15) << val.soChuyen
                 << setw(20) << val.tongCuoc << endl;
        }
        pause();
    }

    void thongKeTongHop() {
        while (true) {
            int c = showMenu(
                "Thong ke doanh thu & hieu suat",
                {
                    "1. Theo ngay/thang/quy",
                    "2. Theo tai xe (so chuyen, quang duong TB, doanh thu)",
                    "3. Theo taxi",
                    "4. Quay lai"
                }
            );
            if (c == -1 || c == 3) break;
            switch (c) {
                case 0: thongKeTheoThoiGian(); break;
                case 1: thongKeTheoTaiXe(); break;
                case 2: thongKeTheoTaxi(); break;
            }
        }
    }

    void menu() {
        if (!dangNhap()) {
            system("cls");
            setColor(10); cout << "Tam biet!\n"; setColor(7);
            return;
        }

        while (true) {
            if (currentRole == "admin") {
                int mainChoice = showMenu(
                    "HE THONG QUAN LY TAXI",
                    {
                        "1. Quan ly tai xe",
                        "2. Quan ly taxi",
                        "3. Quan ly chuyen xe",
                        "4. Thong ke doanh thu",
                        "5. Thoat"
                    }
                );

                if (mainChoice == -1 || mainChoice == 4) {
                    system("cls");
                    setColor(10); cout << "Tam biet!\n"; setColor(7);
                    break;
                }

                if (mainChoice == 0) { // Quan ly tai xe
                    while (true) {
                        int c = showMenu(
                            "Quan ly tai xe",
                            {
								"1. Them tai xe", 
								"2. Hien thi danh sach tai xe", 
								"3. Tim kiem tai xe", 
								"4. Quay lai"
							}
                        );
                        if (c == -1 || c == 3) break;
                        switch (c) {
                            case 0: themTaiXe(); break;
                            case 1: hienThiTaiXe(); break;
                            case 2: timTaiXe(); break;
                        }
                    }
                } else if (mainChoice == 1) { // Quan ly taxi
                    while (true) {
                        int c = showMenu(
                            "Quan ly taxi",
                            {
								"1. Them taxi", 
								"2. Hien thi danh sach taxi", 
								"3. Gan tai xe cho taxi", 
								"4. Quay lai"
							}
                        );
                        if (c == -1 || c == 3) break;
                        switch (c) {
                            case 0: themTaxi(); break;
                            case 1: hienThiTaxi(); break;
                            case 2: ganTaiXeChoTaxi(); break;
                        }
                    }
                } else if (mainChoice == 2) { // Quan ly chuyen xe
                    while (true) {
                        int c = showMenu(
                            "Quan ly chuyen xe",
                            {
								"1. Them chuyen xe", 
								"2. Hien thi danh sach chuyen xe", 
								"3. Quay lai"
							}
                        );
                        if (c == -1 || c == 2) break;
                        switch (c) {
                            case 0: themChuyenXe(); break;
                            case 1: hienThiChuyenXe(); break;
                        }
                    }
                }
                else if (mainChoice == 3) { 
    				thongKeTongHop(); 
				}

            } else { // User role
                int choice = showMenu(
                    "HE THONG QUAN LY TAXI - USER",
                    {"Thoat"}
                );
                if (choice == -1 || choice == 0) {
                    system("cls");
                    setColor(10); cout << "Tam biet!\n"; setColor(7);
                    break;
                }
                system("cls");
                setColor(12); cout << "Chuc nang dat chuyen chua duoc trien khai!\n"; setColor(7);
                pause();
            }
        }
    }
};

int main() {
    QuanLyTaxi ql;
    ql.menu();
    return 0;
}