#include <string>
#include <iostream>

class TaiXe {
public:
    string ID, tenTaiXe, ngaySinh, sdt, bangLai;
    bool trangThai;
    TaiXe(const string&, const string& = "", const string& = "",
          const string& = "", const string& = "", bool = true);
};

