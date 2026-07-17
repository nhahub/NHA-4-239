document.addEventListener("DOMContentLoaded", function() {
    fetchProfileData();
});
 //fetch("../php/get_session.php")
function fetchProfileData() {
    fetch("../php/profile.php") // تأكد من صحة المسار
        .then(response => response.text()) // استخدم text بدلاً من json لعرض المحتوى
        .then(data => {
            console.log(data);  // هذه السطر سوف يعرض محتوى الاستجابة في الـ Console
            try {
                const jsonData = JSON.parse(data);  // محاولة لتحويل المحتوى إلى JSON
                if (jsonData.success) {
                    // عرض اسم المستخدم
                    document.getElementById("userName").textContent = jsonData.user.name;

                    // عرض صورة البروفايل
                    const profilePic = document.getElementById("profilePic");
                    profilePic.src = jsonData.user.profile_pic && jsonData.user.profile_pic.trim() !== ""
                        ? jsonData.user.profile_pic
                        : "../img/profile-pic.jpeg"; // صورة افتراضية

                    // عرض المهارات
                    const skillsContainer = document.getElementById("skillsContainer");
                    const skills = jsonData.user.skills.split(',');
                    skillsContainer.innerHTML = skills.map(skill => `<span class="skill">${skill.trim()}</span>`).join('');

                    // عرض "نبذة عن المستخدم"
                    document.getElementById("aboutMe").textContent = jsonData.user.about_me;

                    // عرض الخدمة المعروضة
                    document.getElementById("serviceOffered").textContent = jsonData.user.service_offered;
                } else {
                    alert(jsonData.message);
                }
            } catch (error) {
                console.error('Error parsing JSON:', error);
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
}