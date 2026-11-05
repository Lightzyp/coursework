//The pattern checks for four capitals, a space, and four digits. for...of gets each course. for...in gets its property names.


export function isValidCourseCode(code) {
    return /^[A-Z]{4} \d{4}$/.test(code);
}

export function isValidEmail(email) {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
}

export function extractCourseCodes(text) {
    return text.match(/\b[A-Z]{4} \d{4}\b/g) ?? [];
}
