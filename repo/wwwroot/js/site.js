const apiBase = "";

let token = localStorage.getItem("hr_token") || "";

let editingDepartmentId = null;
let editingEmployeeId = null;

let departmentsCache = [];
let employeesCache = [];
let leaveRequestsCache = [];

document.addEventListener("DOMContentLoaded", async () => {
    setDefaultDates();

    if (token) {
        openApp();
        await refreshAll();
    }
});

function setDefaultDates() {
    const today = new Date();
    const todayValue = today.toISOString().split("T")[0];

    const employeeHireDate = document.getElementById("employeeHireDate");
    const payrollPeriod = document.getElementById("payrollPeriod");
    const leaveStartDate = document.getElementById("leaveStartDate");
    const leaveEndDate = document.getElementById("leaveEndDate");

    if (employeeHireDate) {
        employeeHireDate.value = todayValue;
    }

    if (payrollPeriod) {
        payrollPeriod.value = today.toISOString().slice(0, 7);
    }

    if (leaveStartDate) {
        leaveStartDate.value = todayValue;
    }

    if (leaveEndDate) {
        leaveEndDate.value = todayValue;
    }
}

function showToast(message) {
    const toast = document.getElementById("toast");

    toast.textContent = message;
    toast.classList.add("show");

    setTimeout(() => {
        toast.classList.remove("show");
    }, 2600);
}

function authHeaders() {
    return {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
    };
}

async function handleResponse(response) {
    const text = await response.text();

    let data = null;

    try {
        data = text ? JSON.parse(text) : null;
    } catch {
        data = text;
    }

    if (!response.ok) {
        if (response.status === 401) {
            throw new Error("Oturum süresi dolmuş olabilir. Lütfen tekrar giriş yapın.");
        }

        if (response.status === 403) {
            throw new Error("Bu işlem için yetkiniz bulunmuyor.");
        }

        if (Array.isArray(data)) {
            throw new Error(data.join(", "));
        }

        if (typeof data === "string") {
            throw new Error(data);
        }

        throw new Error("İşlem sırasında hata oluştu.");
    }

    return data;
}

function openApp() {
    document.getElementById("authPage").classList.add("hidden");
    document.getElementById("appLayout").classList.remove("hidden");
}

async function showPage(pageId, button) {
    document.querySelectorAll(".page").forEach(page => {
        page.classList.remove("active-page");
    });

    document.getElementById(pageId).classList.add("active-page");

    document.querySelectorAll(".nav-btn").forEach(btn => {
        btn.classList.remove("active");
    });

    button.classList.add("active");

    if (pageId === "dashboardPage") {
        await refreshAll();
    }

    if (pageId === "departmentsPage") {
        await loadDepartments();
    }

    if (pageId === "employeesPage") {
        await loadDepartments();
        await loadEmployees();
    }

    if (pageId === "payrollPage") {
        await loadEmployees();
        await loadPayrolls();
    }

    if (pageId === "leaveRequestsPage") {
        await loadEmployees();
        await loadLeaveRequests();
    }
}

async function registerUser() {
    const body = {
        fullName: document.getElementById("registerFullName").value,
        email: document.getElementById("registerEmail").value,
        password: document.getElementById("registerPassword").value
    };

    try {
        const response = await fetch(`${apiBase}/api/Auth/register`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(body)
        });

        const data = await handleResponse(response);

        token = data.token;
        localStorage.setItem("hr_token", token);

        openApp();
        await refreshAll();

        showToast("Kayıt başarılı, sisteme giriş yapıldı.");
    } catch (error) {
        showToast(error.message);
    }
}

async function loginUser() {
    const body = {
        email: document.getElementById("loginEmail").value,
        password: document.getElementById("loginPassword").value
    };

    try {
        const response = await fetch(`${apiBase}/api/Auth/login`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(body)
        });

        const data = await handleResponse(response);

        token = data.token;
        localStorage.setItem("hr_token", token);

        openApp();
        await refreshAll();

        showToast("Giriş başarılı.");
    } catch (error) {
        showToast(error.message);
    }
}

function logout() {
    token = "";
    localStorage.removeItem("hr_token");

    editingDepartmentId = null;
    editingEmployeeId = null;

    document.getElementById("appLayout").classList.add("hidden");
    document.getElementById("authPage").classList.remove("hidden");

    showToast("Çıkış yapıldı.");
}

async function refreshAll() {
    await loadDepartments();
    await loadEmployees();
    await loadPayrolls();
    await loadLeaveRequests();
}

function setTextIfExists(elementId, value) {
    const element = document.getElementById(elementId);

    if (element) {
        element.textContent = value;
    }
}

function updateLeaveSummaryCards(leaveRequests) {
    const totalLeaveCount = leaveRequests.length;
    const pendingLeaveCount = leaveRequests.filter(leave => leave.status === "Beklemede").length;
    const approvedLeaveCount = leaveRequests.filter(leave => leave.status === "Onaylandı").length;
    const rejectedLeaveCount = leaveRequests.filter(leave => leave.status === "Reddedildi").length;

    setTextIfExists("leaveRequestCount", totalLeaveCount);
    setTextIfExists("pendingLeaveCount", pendingLeaveCount);
    setTextIfExists("approvedLeaveCount", approvedLeaveCount);
    setTextIfExists("rejectedLeaveCount", rejectedLeaveCount);
}

async function loadDepartments() {
    try {
        const response = await fetch(`${apiBase}/api/Departments`, {
            headers: authHeaders()
        });

        const departments = await handleResponse(response);

        departmentsCache = departments;

        const list = document.getElementById("departmentList");
        const employeeDepartment = document.getElementById("employeeDepartment");

        list.innerHTML = "";
        employeeDepartment.innerHTML = "";

        document.getElementById("departmentCount").textContent = departments.length;

        if (departments.length === 0) {
            list.innerHTML = "<p>Henüz departman kaydı bulunmuyor.</p>";
            employeeDepartment.innerHTML = "<option value=''>Önce departman ekleyin</option>";
            return;
        }

        departments.forEach(department => {
            const item = document.createElement("div");
            item.className = "item";

            item.innerHTML = `
                <strong>${department.name}</strong>
                <p>${department.description || "Açıklama eklenmemiş"}</p>
                <p>Çalışan Sayısı: ${department.employeeCount}</p>
                <div class="item-actions">
                    <button class="edit-btn" onclick="editDepartment(${department.id})">Düzenle</button>
                    <button class="delete-btn" onclick="deleteDepartment(${department.id})">Sil</button>
                </div>
            `;

            list.appendChild(item);

            const option = document.createElement("option");
            option.value = department.id;
            option.textContent = department.name;
            employeeDepartment.appendChild(option);
        });
    } catch (error) {
        showToast(error.message);
    }
}

async function saveDepartment() {
    if (editingDepartmentId) {
        await updateDepartment();
    } else {
        await addDepartment();
    }
}

async function addDepartment() {
    const body = {
        name: document.getElementById("departmentName").value,
        description: document.getElementById("departmentDescription").value
    };

    try {
        const response = await fetch(`${apiBase}/api/Departments`, {
            method: "POST",
            headers: authHeaders(),
            body: JSON.stringify(body)
        });

        await handleResponse(response);

        clearDepartmentForm();

        await loadDepartments();
        await loadEmployees();

        showToast("Departman başarıyla eklendi.");
    } catch (error) {
        showToast(error.message);
    }
}

function editDepartment(id) {
    const department = departmentsCache.find(d => d.id === id);

    if (!department) {
        showToast("Düzenlenecek departman bulunamadı.");
        return;
    }

    editingDepartmentId = id;

    document.getElementById("departmentName").value = department.name;
    document.getElementById("departmentDescription").value = department.description || "";

    document.getElementById("departmentSaveBtn").textContent = "Güncelle";
    document.getElementById("departmentCancelBtn").classList.remove("hidden");

    showToast("Departman düzenleme moduna alındı.");
}

async function updateDepartment() {
    const body = {
        name: document.getElementById("departmentName").value,
        description: document.getElementById("departmentDescription").value
    };

    try {
        const response = await fetch(`${apiBase}/api/Departments/${editingDepartmentId}`, {
            method: "PUT",
            headers: authHeaders(),
            body: JSON.stringify(body)
        });

        await handleResponse(response);

        cancelDepartmentEdit();

        await loadDepartments();
        await loadEmployees();

        showToast("Departman başarıyla güncellendi.");
    } catch (error) {
        showToast(error.message);
    }
}

function cancelDepartmentEdit() {
    editingDepartmentId = null;

    clearDepartmentForm();

    document.getElementById("departmentSaveBtn").textContent = "Kaydet";
    document.getElementById("departmentCancelBtn").classList.add("hidden");
}

function clearDepartmentForm() {
    document.getElementById("departmentName").value = "";
    document.getElementById("departmentDescription").value = "";
}

async function deleteDepartment(id) {
    if (!confirm("Departmanı silmek istediğinize emin misiniz?")) {
        return;
    }

    try {
        const response = await fetch(`${apiBase}/api/Departments/${id}`, {
            method: "DELETE",
            headers: authHeaders()
        });

        await handleResponse(response);

        await loadDepartments();
        await loadEmployees();

        showToast("Departman silindi.");
    } catch (error) {
        showToast(error.message);
    }
}

async function loadEmployees() {
    try {
        const response = await fetch(`${apiBase}/api/Employees`, {
            headers: authHeaders()
        });

        const employees = await handleResponse(response);

        employeesCache = employees;

        const list = document.getElementById("employeeList");
        const payrollEmployee = document.getElementById("payrollEmployee");
        const leaveEmployee = document.getElementById("leaveEmployee");

        list.innerHTML = "";
        payrollEmployee.innerHTML = "";

        if (leaveEmployee) {
            leaveEmployee.innerHTML = "";
        }

        document.getElementById("employeeCount").textContent = employees.length;

        if (employees.length === 0) {
            list.innerHTML = "<p>Henüz çalışan kaydı bulunmuyor.</p>";
            payrollEmployee.innerHTML = "<option value=''>Önce çalışan ekleyin</option>";

            if (leaveEmployee) {
                leaveEmployee.innerHTML = "<option value=''>Önce çalışan ekleyin</option>";
            }

            return;
        }

        employees.forEach(employee => {
            const item = document.createElement("div");
            item.className = "item";

            item.innerHTML = `
                <strong>${employee.fullName}</strong>
                <p>E-posta: ${employee.email}</p>
                <p>Telefon: ${employee.phoneNumber || "-"}</p>
                <p>Departman: ${employee.departmentName}</p>
                <p>Brüt Maaş: ${employee.grossSalary} TL</p>
                <div class="item-actions">
                    <button class="edit-btn" onclick="editEmployee(${employee.id})">Düzenle</button>
                    <button class="delete-btn" onclick="deleteEmployee(${employee.id})">Sil</button>
                </div>
            `;

            list.appendChild(item);

            const payrollOption = document.createElement("option");
            payrollOption.value = employee.id;
            payrollOption.textContent = `${employee.fullName} - ${employee.departmentName}`;
            payrollEmployee.appendChild(payrollOption);

            if (leaveEmployee) {
                const leaveOption = document.createElement("option");
                leaveOption.value = employee.id;
                leaveOption.textContent = `${employee.fullName} - ${employee.departmentName}`;
                leaveEmployee.appendChild(leaveOption);
            }
        });
    } catch (error) {
        showToast(error.message);
    }
}

async function saveEmployee() {
    if (editingEmployeeId) {
        await updateEmployee();
    } else {
        await addEmployee();
    }
}

async function addEmployee() {
    const body = getEmployeeFormData();

    try {
        const response = await fetch(`${apiBase}/api/Employees`, {
            method: "POST",
            headers: authHeaders(),
            body: JSON.stringify(body)
        });

        await handleResponse(response);

        clearEmployeeForm();

        await loadEmployees();
        await loadDepartments();

        showToast("Çalışan başarıyla eklendi.");
    } catch (error) {
        showToast(error.message);
    }
}

function editEmployee(id) {
    const employee = employeesCache.find(e => e.id === id);

    if (!employee) {
        showToast("Düzenlenecek çalışan bulunamadı.");
        return;
    }

    editingEmployeeId = id;

    document.getElementById("employeeFirstName").value = employee.firstName;
    document.getElementById("employeeLastName").value = employee.lastName;
    document.getElementById("employeeEmail").value = employee.email;
    document.getElementById("employeePhone").value = employee.phoneNumber || "";
    document.getElementById("employeeHireDate").value = employee.hireDate.split("T")[0];
    document.getElementById("employeeSalary").value = employee.grossSalary;
    document.getElementById("employeeDepartment").value = employee.departmentId;

    document.getElementById("employeeSaveBtn").textContent = "Güncelle";
    document.getElementById("employeeCancelBtn").classList.remove("hidden");

    showToast("Çalışan düzenleme moduna alındı.");
}

async function updateEmployee() {
    const body = getEmployeeFormData();

    try {
        const response = await fetch(`${apiBase}/api/Employees/${editingEmployeeId}`, {
            method: "PUT",
            headers: authHeaders(),
            body: JSON.stringify(body)
        });

        await handleResponse(response);

        cancelEmployeeEdit();

        await loadEmployees();
        await loadDepartments();

        showToast("Çalışan başarıyla güncellendi.");
    } catch (error) {
        showToast(error.message);
    }
}

function getEmployeeFormData() {
    return {
        firstName: document.getElementById("employeeFirstName").value,
        lastName: document.getElementById("employeeLastName").value,
        email: document.getElementById("employeeEmail").value,
        phoneNumber: document.getElementById("employeePhone").value,
        hireDate: document.getElementById("employeeHireDate").value,
        grossSalary: Number(document.getElementById("employeeSalary").value),
        departmentId: Number(document.getElementById("employeeDepartment").value)
    };
}

function cancelEmployeeEdit() {
    editingEmployeeId = null;

    clearEmployeeForm();

    document.getElementById("employeeSaveBtn").textContent = "Kaydet";
    document.getElementById("employeeCancelBtn").classList.add("hidden");
}

function clearEmployeeForm() {
    document.getElementById("employeeFirstName").value = "";
    document.getElementById("employeeLastName").value = "";
    document.getElementById("employeeEmail").value = "";
    document.getElementById("employeePhone").value = "";
    document.getElementById("employeeSalary").value = "";
}

async function deleteEmployee(id) {
    if (!confirm("Çalışanı silmek istediğinize emin misiniz?")) {
        return;
    }

    try {
        const response = await fetch(`${apiBase}/api/Employees/${id}`, {
            method: "DELETE",
            headers: authHeaders()
        });

        await handleResponse(response);

        await loadEmployees();
        await loadDepartments();
        await loadPayrolls();
        await loadLeaveRequests();

        showToast("Çalışan silindi.");
    } catch (error) {
        showToast(error.message);
    }
}

async function calculatePayroll() {
    const employeeId = Number(document.getElementById("payrollEmployee").value);
    const period = document.getElementById("payrollPeriod").value;

    if (!employeeId) {
        showToast("Bordro hesaplamak için çalışan seçmelisiniz.");
        return;
    }

    const selectedEmployee = employeesCache.find(e => e.id === employeeId);

    if (selectedEmployee) {
        const hireMonth = selectedEmployee.hireDate.slice(0, 7);

        if (period < hireMonth) {
            showToast(`Bu çalışan ${hireMonth} döneminden önce bordro alamaz.`);
            return;
        }
    }

    try {
        const response = await fetch(`${apiBase}/api/Payrolls/calculate/${employeeId}`, {
            method: "POST",
            headers: authHeaders(),
            body: JSON.stringify({ period })
        });

        await handleResponse(response);

        await loadPayrolls();

        showToast("Bordro başarıyla hesaplandı.");
    } catch (error) {
        showToast(error.message);
    }
}

async function loadPayrolls() {
    try {
        const response = await fetch(`${apiBase}/api/Payrolls`, {
            headers: authHeaders()
        });

        const payrolls = await handleResponse(response);

        const list = document.getElementById("payrollList");

        list.innerHTML = "";

        document.getElementById("payrollCount").textContent = payrolls.length;

        if (payrolls.length === 0) {
            list.innerHTML = "<p>Henüz bordro kaydı bulunmuyor.</p>";
            return;
        }

        payrolls.forEach(payroll => {
            const item = document.createElement("div");
            item.className = "item";

            item.innerHTML = `
                <strong>${payroll.employeeFullName}</strong>
                <p>Dönem: ${payroll.period}</p>
                <p>Brüt Maaş: ${payroll.grossSalary} TL</p>
                <p>Kesinti: ${payroll.deductionAmount} TL</p>
                <p>Net Maaş: ${payroll.netSalary} TL</p>
                <div class="item-actions">
                    <button class="edit-btn" onclick="downloadPayrollPdf(${payroll.id})">PDF İndir</button>
                    <button class="delete-btn" onclick="deletePayroll(${payroll.id})">Sil</button>
                </div>
            `;

            list.appendChild(item);
        });
    } catch (error) {
        showToast(error.message);
    }
}

async function downloadPayrollPdf(id) {
    try {
        const response = await fetch(`${apiBase}/api/Payrolls/${id}/pdf`, {
            method: "GET",
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        if (!response.ok) {
            const errorText = await response.text();

            if (response.status === 401) {
                throw new Error("Oturum süresi dolmuş olabilir. Lütfen tekrar giriş yapın.");
            }

            if (response.status === 403) {
                throw new Error("Bu işlem için yetkiniz bulunmuyor.");
            }

            throw new Error(errorText || "PDF indirme sırasında hata oluştu.");
        }

        const blob = await response.blob();
        const url = window.URL.createObjectURL(blob);

        const link = document.createElement("a");
        link.href = url;
        link.download = `bordro-${id}.pdf`;

        document.body.appendChild(link);
        link.click();
        link.remove();

        window.URL.revokeObjectURL(url);

        showToast("Bordro PDF dosyası indirildi.");
    } catch (error) {
        showToast(error.message);
    }
}

async function deletePayroll(id) {
    if (!confirm("Bu bordro kaydını silmek istediğinize emin misiniz?")) {
        return;
    }

    try {
        const response = await fetch(`${apiBase}/api/Payrolls/${id}`, {
            method: "DELETE",
            headers: authHeaders()
        });

        await handleResponse(response);

        await loadPayrolls();

        showToast("Bordro kaydı silindi.");
    } catch (error) {
        showToast(error.message);
    }
}

function formatDate(dateValue) {
    if (!dateValue) {
        return "-";
    }

    return new Date(dateValue).toLocaleDateString("tr-TR");
}

function getStatusClass(status) {
    if (status === "Onaylandı") {
        return "status-approved";
    }

    if (status === "Reddedildi") {
        return "status-rejected";
    }

    return "status-pending";
}

async function loadLeaveRequests() {
    const list = document.getElementById("leaveRequestList");

    try {
        const response = await fetch(`${apiBase}/api/LeaveRequests`, {
            headers: authHeaders()
        });

        const leaveRequests = await handleResponse(response);

        leaveRequestsCache = leaveRequests;
        updateLeaveSummaryCards(leaveRequests);

        if (!list) {
            return;
        }

        list.innerHTML = "";

        if (leaveRequests.length === 0) {
            list.innerHTML = "<p>Henüz izin talebi bulunmuyor.</p>";
            return;
        }

        leaveRequests.forEach(leave => {
            const item = document.createElement("div");
            item.className = "item";

            item.innerHTML = `
                <strong>${leave.employeeFullName}</strong>
                <p>İzin Türü: ${leave.leaveType}</p>
                <p>Tarih: ${formatDate(leave.startDate)} - ${formatDate(leave.endDate)}</p>
                <p>Toplam Gün: ${leave.totalDays}</p>
                <p>Açıklama: ${leave.description || "-"}</p>
                <p>Durum: <span class="status-badge ${getStatusClass(leave.status)}">${leave.status}</span></p>
                <div class="item-actions">
                    <button class="edit-btn" onclick="updateLeaveRequestStatus(${leave.id}, 'Onaylandı')">Onayla</button>
                    <button class="delete-btn" onclick="updateLeaveRequestStatus(${leave.id}, 'Reddedildi')">Reddet</button>
                    <button class="delete-btn" onclick="deleteLeaveRequest(${leave.id})">Sil</button>
                </div>
            `;

            list.appendChild(item);
        });
    } catch (error) {
        showToast(error.message);
    }
}

async function saveLeaveRequest() {
    const employeeId = Number(document.getElementById("leaveEmployee").value);
    const leaveType = document.getElementById("leaveType").value;
    const startDate = document.getElementById("leaveStartDate").value;
    const endDate = document.getElementById("leaveEndDate").value;
    const description = document.getElementById("leaveDescription").value;

    if (!employeeId) {
        showToast("İzin talebi oluşturmak için çalışan seçmelisiniz.");
        return;
    }

    if (!startDate || !endDate) {
        showToast("Başlangıç ve bitiş tarihi seçmelisiniz.");
        return;
    }

    if (endDate < startDate) {
        showToast("Bitiş tarihi başlangıç tarihinden önce olamaz.");
        return;
    }

    const body = {
        employeeId,
        leaveType,
        startDate,
        endDate,
        description
    };

    try {
        const response = await fetch(`${apiBase}/api/LeaveRequests`, {
            method: "POST",
            headers: authHeaders(),
            body: JSON.stringify(body)
        });

        await handleResponse(response);

        clearLeaveRequestForm();

        await loadLeaveRequests();

        showToast("İzin talebi başarıyla oluşturuldu.");
    } catch (error) {
        showToast(error.message);
    }
}

function clearLeaveRequestForm() {
    const leaveDescription = document.getElementById("leaveDescription");
    const leaveStartDate = document.getElementById("leaveStartDate");
    const leaveEndDate = document.getElementById("leaveEndDate");

    if (leaveDescription) {
        leaveDescription.value = "";
    }

    const todayValue = new Date().toISOString().split("T")[0];

    if (leaveStartDate) {
        leaveStartDate.value = todayValue;
    }

    if (leaveEndDate) {
        leaveEndDate.value = todayValue;
    }
}

async function updateLeaveRequestStatus(id, status) {
    try {
        const response = await fetch(`${apiBase}/api/LeaveRequests/${id}/status`, {
            method: "PUT",
            headers: authHeaders(),
            body: JSON.stringify({ status })
        });

        await handleResponse(response);

        await loadLeaveRequests();

        showToast(`İzin durumu ${status} olarak güncellendi.`);
    } catch (error) {
        showToast(error.message);
    }
}

async function deleteLeaveRequest(id) {
    if (!confirm("Bu izin talebini silmek istediğinize emin misiniz?")) {
        return;
    }

    try {
        const response = await fetch(`${apiBase}/api/LeaveRequests/${id}`, {
            method: "DELETE",
            headers: authHeaders()
        });

        await handleResponse(response);

        await loadLeaveRequests();

        showToast("İzin talebi silindi.");
    } catch (error) {
        showToast(error.message);
    }
}