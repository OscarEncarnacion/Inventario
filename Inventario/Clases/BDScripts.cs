using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario.Clases
{
    internal class BDScripts
    {
        private readonly Dictionary<string, string> sample = new()
        {
            {"ID", "ID_SAMPLE"},
            {"Serial Number", "SERIAL_NUMBER"},
            {"Hardware Level", "HW_LEVEL"},
            {"Software Level", "SW_LEVEL"},
            {"Delivered To", "DELIVERED_TO"},
            {"Discipline", "DISCIPLINEA"},
            {"Functional Test", "FUNCTIONAL_TEST"},
            {"Rework Sample", "REWORK_SAMPLE"},
            {"Rework Connector", "REWORK_CONNECTOR"},
            {"APTIV Connector", "APTIV_CONNECTOR"},
            {"Comments", "COMMENTS"},
            {"Serial Case", "SERIAL_CASE"},
            {"Date Delivery", "DATE_DELIVERY"},
            {"Project", "PROJECT"},
            {"Business Area - Location", "BALOCATION"},
            {"Variant", "VARIANT"},
            {"Job", "JOB"},
            {"Type Test", "TYPE_TEST"},
            {"Creator user", "IUSER"}
        };
        private readonly Dictionary<string, string> VNs = new()
        {
            {"ID", "ID_VN"},
            {"Serial Number", "SERIAL_NUMBER"},
            {"TAG", "TAG"},
            {"Model", "MODEL"},
            {"Type License", "TYPE_LICENSE"},
            {"BA-Location", "BALOCATION"},
            {"Project", "PROJECT"},
            {"Responsible", "RESPONSIBLE"},
            {"Current Assigned", "CURRENT_ASSIGNED"},
            {"Comments", "COMMENTS"},
            {"Link", "LINK"},
            {"Creator user", "IUSER"},
            {"Date Reception", "DATE_RECEPTION"},
            {"Date Load", "DATE_LOAD"}
        };
        private readonly Dictionary<string, string> discipline = new()
        {
            {"ID", "ID_DISCIPLINE"},
            {"Acronym", "ACRONYM"},
            {"Description", "DESCRIPTION"}
        };
        private readonly Dictionary<string, string> project = new()
        {
            {"ID", "ID_PROJECT"},
            {"Acronym", "ACRONYM"},
            {"Description", "DESCRIPTION"}
        };
        private readonly Dictionary<string, string> location = new()
        {
            {"ID", "ID_LOCATION"},
            {"Acronym", "ACRONYM"},
            {"Description", "DESCRIPTION"}
        };
        private readonly Dictionary<string, string> BA = new()
        {
            {"ID", "ID_BA"},
            {"Acronym", "ACRONYM"},
            {"Description", "DESCRIPTION"}
        };
        private readonly Dictionary<string, string> variant = new()
        {
            {"ID", "ID_VARIANT"},
            {"Acronym", "ACRONYM"},
            {"Description", "DESCRIPTION"}
        };
        private readonly Dictionary<string, string> user = new()
        {
            {"ID", "ID_USER"},
            {"Name", "NAMEL"},
            {"Employee Number", "EMPLOYEE_NUMBER"},
            {"Email", "EMAIL"},
            {"Business Area - Location", "BALOCATION"},
            {"Privilege level", "ROLE"}
        };
        private readonly Dictionary<string, string> BALocation = new()
        {
            {"ID", "ID_BA_LOCATION"},
            {"Business Area", "BA"},
            {"Location", "LOCATION"}
        };

        public BDScripts() { }

        public Dictionary<string, string> GetDataTableInformation(string table)
        {
            return table switch
            {
                "SAMPLE" => sample,
                "VN" => VNs,
                "DISCIPLINE" => discipline,
                "PROJECT" => project,
                "LOCATION" => location,
                "BUSINESS_AREA" => BA,
                "VARIANT" => variant,
                "INVENTORY_USERS" => user,
                "BA_LOCATION" => BALocation,
                _ => sample,
            };
        }

        public string Inserts(string table)
        {
            return table switch
            {
                "user" => "INSERT INTO INVENTORY_USERS (NAME, LAST_NAME, EMPLOYEE_NUMBER, EMAIL, ID_BA_LOCATION, PASSWORD, ROLE, " +
                "STATUS) VALUES (@Name, @LastName, @EmployeeNumber, @Email, @ID_BA_LOCATION, @Password, @Role, @Status)",
                "sample" => "INSERT INTO SAMPLE (SERIAL_NUMBER, HW_LEVEL, SW_LEVEL, DELIVERED_TO, ID_DISCIPLINE, FUNCTIONAL_TEST, " +
                "REWORK_SAMPLE, REWORK_CONNECTOR, APTIV_CONNECTOR, COMMENTS, SERIAL_CASE, DATE_DELIVERY, ID_PROJECT, ID_BA_LOCATION, " +
                "ID_VARIANT, JOB, ID_TYPE_TEST, ID_USER) VALUES (@SN, @HW, @SW, @DT, @DI, @FT, @RS, @RC, @AC, @CO, @SC, @DD, " +
                "@IP, @IBL, @IV, @JO, @ITT, @IU)",
                "vn" => "INSERT INTO VN (SERIAL_NUMBER, TAG, MODEL, TYPE_LICENSE, ID_BA_LOCATION, ID_PROJECT, RESPONSIBLE, CURRENT_ASSIGNED, " +
                "COMMENTS, LINK, ID_USER, DATE_RECEPTION, DATE_LOAD) VALUES (@SN, @TA, @MO, @TL, @IBL, @IP, @RE, @CA, @CO, @LI, @IU, @DR, @DL)",
                "discipline" => "INSERT INTO DISCIPLINE (ACRONYM, DESCRIPTION) VALUES (@AC, @DE)",
                "project" => "INSERT INTO PROJECT (ACRONYM, DESCRIPTION) VALUES (@AC, @DE)",
                "businessArea" => "INSERT INTO BUSINESS_AREA (ACRONYM, DESCRIPTION) VALUES (@AC, @DE)",
                "baLocation" => "INSERT INTO BA_LOCATION (ID_BA, ID_LOCATION) VALUES (@IBA, @IL)",
                "location" => "INSERT INTO LOCATION (ACRONYM, DESCRIPTION) VALUES (@AC, @DE)",
                "variant" => "INSERT INTO VARIANT (ACRONYM, DESCRIPTION) VALUES (@AC, @DE)",
                "typeTest" => "INSERT INTO TYPE_TEST (DESCRIPTION) VALUES (@DE)",
                _ => String.Empty
            };
        }

        public string SelectOwner(string table)
        {
            switch (table)
            {
                case "SAMPLE":
                    return "SELECT ID_USER FROM SAMPLE WHERE ID_SAMPLE = @ID";
                case "VN":
                    return "SELECT ID_USER FROM VN WHERE ID_VN = @ID";
                default:
                    return "";
            }
        }

        public string SelectUser(string selectType)
        {
            return selectType switch
            {
                "login" => "SELECT TOP 1 PASSWORD, ID_USER, NAME, LAST_NAME, ROLE FROM INVENTORY_USERS WHERE EMPLOYEE_NUMBER = @EN",
                "password" => "SELECT TOP 1 PASSWORD FROM INVENTORY_USERS WHERE ID_USER = @ID",
                "employeeNumber" => "SELECT TOP 1 ID_USER, NAME FROM INVENTORY_USERS WHERE EMPLOYEE_NUMBER = @EN",
                "email" => "SELECT TOP 1 ID_USER, EMAIL FROM INVENTORY_USERS WHERE EMAIL = @EM",
                "edit" => "SELECT TOP 1 NAME, LAST_NAME, EMPLOYEE_NUMBER, EMAIL, ID_BA_LOCATION, ROLE, STATUS FROM INVENTORY_USERS WHERE ID_USER = @ID",
                "balocation" => "SELECT TOP 1 ID_BA_LOCATION FROM INVENTORY_USERS WHERE ID_USER = @ID",
                _ => String.Empty
            };
        }

        public string SelectUserRole()
        {
            return "SELECT TOP 1 ROLE FROM INVENTORY_USERS WHERE ID_USER = @ID";
        }

        public string SelectBusinessAreaLocation()
        {
            return "SELECT ID_BA_LOCATION, CONCAT(b.ACRONYM, ' (', l.ACRONYM, '-', l.DESCRIPTION, ')') AS BALOCATION FROM BA_LOCATION AS ba " +
                "INNER JOIN BUSINESS_AREA AS b ON b.ID_BA = ba.ID_BA " +
                "INNER JOIN LOCATION AS l ON l.ID_LOCATION = ba.ID_LOCATION ORDER BY BALOCATION";
        }

        public string SelectProject()
        {
            return "SELECT ID_PROJECT, ACRONYM FROM PROJECT ORDER BY ACRONYM";
        }

        public string SelectBusinessArea()
        {
            return "SELECT ID_BA, CONCAT(ACRONYM, '-', DESCRIPTION) AS ACDE FROM BUSINESS_AREA ORDER BY ACDE";
        }

        public string SelectLocation()
        {
            return "SELECT ID_LOCATION, CONCAT(DESCRIPTION, '-', ACRONYM) AS DEAC FROM LOCATION ORDER BY DEAC";
        }

        public string SelectDiscipline()
        {
            return "SELECT ID_DISCIPLINE, ACRONYM FROM DISCIPLINE ORDER BY ACRONYM";
        }

        public string SelectVariant()
        {
            return "SELECT ID_VARIANT, DESCRIPTION FROM VARIANT ORDER BY DESCRIPTION";
        }

        public string SelectTypeTest()
        {
            return "SELECT ID_TYPE_TEST, DESCRIPTION FROM TYPE_TEST ORDER BY DESCRIPTION";
        }

        public string SelectSample(string selectType)
        {
            return selectType switch
            {
                "serialNumber" => "SELECT TOP 1 ID_SAMPLE FROM SAMPLE WHERE SERIAL_NUMBER = @SN",
                "form" => "SELECT TOP 1 SERIAL_NUMBER, HW_LEVEL, SW_LEVEL, DELIVERED_TO, ID_DISCIPLINE, FUNCTIONAL_TEST, REWORK_SAMPLE, REWORK_CONNECTOR, " +
                                        "APTIV_CONNECTOR, COMMENTS, SERIAL_CASE, DATE_DELIVERY, ID_PROJECT, ID_BA_LOCATION, ID_VARIANT, JOB, ID_TYPE_TEST FROM SAMPLE WHERE ID_SAMPLE = @ID",
                _ => String.Empty
            };
        }

        public string SelectVN(string selectType)
        {
            return selectType switch
            {
                "serialNumber" => "SELECT TOP 1 ID_VN FROM VN WHERE SERIAL_NUMBER = @SN",
                "form" => "SELECT TOP 1 SERIAL_NUMBER, TAG, MODEL, TYPE_LICENSE, ID_BA_LOCATION, ID_PROJECT, RESPONSIBLE, CURRENT_ASSIGNED, COMMENTS, " +
                "LINK, ID_USER, DATE_RECEPTION, DATE_LOAD FROM VN WHERE ID_VN = @ID",
                _ => String.Empty
            };
        }

        public string SelectDiscipline(string selectType)
        {
            return selectType switch
            {
                "acronym" => "SELECT TOP 1 ID_DISCIPLINE FROM DISCIPLINE WHERE ACRONYM = @AC",
                "form" => "SELECT TOP 1 ACRONYM, DESCRIPTION FROM DISCIPLINE WHERE ID_DISCIPLINE = @ID",
                _ => String.Empty
            };
        }

        public string SelectProject(string selectType)
        {
            return selectType switch
            {
                "acronym" => "SELECT TOP 1 ID_PROJECT FROM PROJECT WHERE ACRONYM = @AC",
                "form" => "SELECT TOP 1 ACRONYM, DESCRIPTION FROM PROJECT WHERE ID_PROJECT = @ID",
                _ => String.Empty
            };
        }

        public string SelectBusinessArea(string selectType)
        {
            return selectType switch
            {
                "acronym" => "SELECT TOP 1 ID_BA FROM BUSINESS_AREA WHERE ACRONYM = @AC",
                "form" => "SELECT TOP 1 ACRONYM, DESCRIPTION FROM BUSINESS_AREA WHERE ID_BA = @ID",
                _ => String.Empty
            };
        }

        public string SelectLocation(string selectType)
        {
            return selectType switch
            {
                "acronym" => "SELECT TOP 1 ID_LOCATION FROM LOCATION WHERE ACRONYM = @AC",
                "form" => "SELECT TOP 1 ACRONYM, DESCRIPTION FROM LOCATION WHERE ID_LOCATION = @ID",
                _ => String.Empty
            };
        }

        public string SelectVariant(string selectType)
        {
            return selectType switch
            {
                "acronym" => "SELECT TOP 1 ID_VARIANT FROM VARIANT WHERE ACRONYM = @AC",
                "form" => "SELECT TOP 1 ACRONYM, DESCRIPTION FROM VARIANT WHERE ID_VARIANT = @ID",
                _ => String.Empty
            };
        }

        public string SelectTypeTest(string selectType)
        {
            return selectType switch
            {
                "description" => "SELECT TOP 1 ID_TYPE_TEST FROM TYPE_TEST WHERE DESCRIPTION = @DE",
                "form" => "SELECT TOP 1 DESCRIPTION FROM TYPE_TEST WHERE ID_TYPE_TEST = @ID",
                _ => String.Empty
            };
        }

        public string SelectBAL(string selectType)
        {
            return selectType switch
            {
                "baLocation" => "SELECT TOP 1 ID_BA_LOCATION FROM BA_LOCATION WHERE ID_BA = @IBA AND ID_LOCATION = @IL",
                "form" => "SELECT TOP 1 ID_BA, ID_LOCATION FROM BA_LOCATION WHERE ID_BA_LOCATION = @ID",
                _ => String.Empty
            };
        }

        public string SelectTable(string table)
        {
            return table switch
            {
                // Sample
                "SAMPLE" => "SELECT ID_SAMPLE, SERIAL_NUMBER, HW_LEVEL, SW_LEVEL, DELIVERED_TO, di.ACRONYM AS DISCIPLINEA, " +
                "FUNCTIONAL_TEST, REWORK_SAMPLE, REWORK_CONNECTOR, APTIV_CONNECTOR, COMMENTS, SERIAL_CASE, DATE_DELIVERY, " +
                "pr.ACRONYM AS PROJECT, CONCAT(ba.ACRONYM, ' (', lo.ACRONYM, '-', lo.DESCRIPTION, ')') AS BALOCATION, " +
                "va.ACRONYM AS VARIANT, JOB, tt.DESCRIPTION AS TYPE_TEST, CONCAT(us.NAME, ' ', us.LAST_NAME) AS IUSER FROM SAMPLE AS sa " +
                "INNER JOIN DISCIPLINE AS di ON di.ID_DISCIPLINE = sa.ID_DISCIPLINE " +
                "INNER JOIN PROJECT AS pr ON pr.ID_PROJECT = sa.ID_PROJECT " +
                "INNER JOIN BA_LOCATION AS bal ON bal.ID_BA_LOCATION = sa.ID_BA_LOCATION " +
                "INNER JOIN BUSINESS_AREA AS ba ON ba.ID_BA = bal.ID_BA " +
                "INNER JOIN LOCATION AS lo ON lo.ID_LOCATION = bal.ID_LOCATION " +
                "INNER JOIN VARIANT AS va ON va.ID_VARIANT = sa.ID_VARIANT " +
                "INNER JOIN TYPE_TEST AS tt ON tt.ID_TYPE_TEST = sa.ID_TYPE_TEST " +
                "INNER JOIN INVENTORY_USERS AS us ON us.ID_USER = sa.ID_USER",
                // VN
                "VN" => "SELECT ID_VN, SERIAL_NUMBER, TAG, MODEL, TYPE_LICENSE, CONCAT(ba.ACRONYM, ' (', lo.ACRONYM, '-', lo.DESCRIPTION, ')') AS BALOCATION, " +
                "pr.ACRONYM AS PROJECT, RESPONSIBLE, CURRENT_ASSIGNED, COMMENTS, LINK, CONCAT(us.NAME, ' ', us.LAST_NAME) AS IUSER, DATE_RECEPTION, DATE_LOAD FROM VN AS vn " +
                "INNER JOIN BA_LOCATION AS bal ON bal.ID_BA_LOCATION = vn.ID_BA_LOCATION " +
                "INNER JOIN BUSINESS_AREA AS ba ON ba.ID_BA = bal.ID_BA " +
                "INNER JOIN LOCATION AS lo ON lo.ID_LOCATION = bal.ID_LOCATION " +
                "INNER JOIN PROJECT AS pr ON pr.ID_PROJECT = vn.ID_PROJECT " +
                "INNER JOIN INVENTORY_USERS AS us ON us.ID_USER = vn.ID_USER",
                // Discipline
                "DISCIPLINE" => "SELECT ID_DISCIPLINE, ACRONYM, DESCRIPTION FROM DISCIPLINE",
                // Project
                "PROJECT" => "SELECT ID_PROJECT, ACRONYM, DESCRIPTION FROM PROJECT",
                // Business area
                "BUSINESS_AREA" => "SELECT ID_BA, ACRONYM, DESCRIPTION FROM BUSINESS_AREA",
                // Location
                "LOCATION" => "SELECT ID_LOCATION, ACRONYM, DESCRIPTION FROM LOCATION",
                // Business area location
                "BA_LOCATION" => "SELECT ID_BA_LOCATION, ba.ACRONYM AS BA, CONCAT(lo.DESCRIPTION, '-', lo.ACRONYM) AS LOCATION FROM BA_LOCATION AS bal " +
                "INNER JOIN BUSINESS_AREA AS ba ON ba.ID_BA = bal.ID_BA " +
                "INNER JOIN LOCATION AS lo ON lo.ID_LOCATION = bal.ID_LOCATION",
                // Variant
                "VARIANT" => "SELECT ID_VARIANT, ACRONYM, DESCRIPTION FROM VARIANT",
                // Type test
                "TYPE_TEST" => "SELECT ID_TYPE_TEST, DESCRIPTION FROM TYPE_TEST",
                // User
                "INVENTORY_USERS" => "SELECT ID_USER, CONCAT(NAME, ' ', LAST_NAME) AS NAMEL, EMPLOYEE_NUMBER, EMAIL, " +
                "CONCAT(ba.ACRONYM, ' (', lo.ACRONYM, '-', lo.DESCRIPTION, ')') AS BALOCATION, ROLE FROM INVENTORY_USERS AS us " +
                "INNER JOIN BA_LOCATION AS bal ON bal.ID_BA_LOCATION = us.ID_BA_LOCATION\r\nINNER JOIN BUSINESS_AREA AS ba ON ba.ID_BA = bal.ID_BA " +
                "INNER JOIN LOCATION AS lo ON lo.ID_LOCATION = bal.ID_LOCATION",
                _ => String.Empty
            };
        }

        public string UpdateUser(string selectType)
        {
            return selectType switch
            {
                "password" => "UPDATE INVENTORY_USERS SET PASSWORD = @PASS WHERE ID_USER = @ID",
                "personalInformation" => "UPDATE INVENTORY_USERS SET NAME = @Name, LAST_NAME = @LastName, EMPLOYEE_NUMBER = @EmployeeNumber, " +
                "EMAIL = @Email, ID_BA_LOCATION = @ID_BA_LOCATION, ROLE = @Role, STATUS = @Status WHERE ID_USER = @ID",
                _ => String.Empty,
            };
        }

        public string UpdateTables(string updateType)
        {
            return updateType switch
            {
                "sample" => "UPDATE SAMPLE SET SERIAL_NUMBER = @SN, HW_LEVEL = @HW, SW_LEVEL = @SW, DELIVERED_TO = @DT, ID_DISCIPLINE = @DI, FUNCTIONAL_TEST = @FT, " +
                "REWORK_SAMPLE = @RS, REWORK_CONNECTOR = @RC, APTIV_CONNECTOR = @AC, COMMENTS = @CO, SERIAL_CASE = @SC, DATE_DELIVERY = @DD, ID_PROJECT = @IP, " +
                "ID_VARIANT = @IV, JOB = @JO, ID_TYPE_TEST = @ITT, ID_USER = @IU WHERE ID_SAMPLE = @ID",
                "vn" => "UPDATE VN SET SERIAL_NUMBER = @SN, TAG = @TA, MODEL = @MO, TYPE_LICENSE = @TL, ID_PROJECT = @IP, RESPONSIBLE = @RE, " +
                "CURRENT_ASSIGNED = @CA, COMMENTS = @CO, LINK = @LI, ID_USER = @IU, DATE_RECEPTION = @DR, DATE_LOAD = @DL WHERE ID_VN = @ID",
                "discipline" => "UPDATE DISCIPLINE SET ACRONYM = @AC, DESCRIPTION = @DE WHERE ID_DISCIPLINE = @ID",
                "project" => "UPDATE PROJECT SET ACRONYM = @AC, DESCRIPTION = @DE WHERE ID_PROJECT = @ID",
                "businessArea" => "UPDATE BUSINESS_AREA SET ACRONYM = @AC, DESCRIPTION = @DE WHERE ID_BA = @ID",
                "location" => "UPDATE LOCATION SET ACRONYM = @AC, DESCRIPTION = @DE WHERE ID_LOCATION = @ID",
                "baLocation" => "UPDATE BA_LOCATION SET ID_BA = @IBA, ID_LOCATION = @IL WHERE ID_BA_LOCATION = @IBAL",
                "variant" => "UPDATE VARIANT SET ACRONYM = @AC, DESCRIPTION = @DE WHERE ID_VARIANT = @ID",
                "typeTest" => "UPDATE TYPE_TEST SET DESCRIPTION = @DE WHERE ID_TYPE_TEST = @ID",
                _ => String.Empty
            };
        }

        public string DeleteCommand(string table)
        {
            string? id;
            switch (table)
            {
                case "SAMPLE":
                    sample.TryGetValue("ID", out id);
                    break;
                case "VN":
                    VNs.TryGetValue("ID", out id);
                    break;
                case "BA_LOCATION":
                    BALocation.TryGetValue("ID", out id);
                    break;
                case "DISCIPLINE":
                    discipline.TryGetValue("ID", out id);
                    break;
                case "PROJECT":
                    project.TryGetValue("ID", out id);
                    break;
                case "LOCATION":
                    location.TryGetValue("ID", out id);
                    break;
                case "BUSINESS_AREA":
                    BA.TryGetValue("ID", out id);
                    break;
                case "VARIANT":
                    variant.TryGetValue("ID", out id);
                    break;
                case "INVENTORY_USERS":
                    user.TryGetValue("ID", out id);
                    break;
                default:
                    id = "";
                    break;
            }
            string command = "DELETE FROM " + table + " WHERE " + id + " = @Id";
            return command;
        }
    }
}
