#include <iostream>
#include <string>
using namespace std;

class ChuyenXe {
public:
    string maChuyen, tenTaiXe, tenKhach, sdtKhach, bienSoTaxi, thoiDiem;
    double khoangCach, thoiGian, cuocPhi;
    ChuyenXe(const string& = "", const string& = "", const string& = "",
             const string& = "", const string& = "", const string& = "",
             double = 0.0, double = 0.0, double = 0.0);
};
