#include "ChuyenXe.h"

ChuyenXe(const string& mc = "", const string& tk = "", const string& ttx = "",
             const string& sdt = "", const string& bs = "", const string& td = "",
             double kc = 0.0, double tg = 0.0, double cp = 0.0)
        : maChuyen(mc), tenKhach(tk), tenTaiXe(ttx), sdtKhach(sdt), bienSoTaxi(bs),
          thoiDiem(td), khoangCach(kc), thoiGian(tg), cuocPhi(cp) {}
