import { jwtDecode } from "jwt-decode";
import api from "./api"; // assuming you already have an axios instance like this

export async function getCurrentUser() {
  const token = localStorage.getItem('token');
  if (!token) return null;

  try {
    const decodedToken = jwtDecode(token);
    const { sub: id, unique_name: login, UserType: role } = decodedToken;
    const response = await api.get(`/users/${id}`);
    const { firstName, lastName, fullName } = response.data;

    // Ensure id is an integer
    const userId = parseInt(id, 10); // Convert to integer

    const name = firstName;
    const surname = lastName;

    return { id: userId, login, role, name, surname, fullName };
  } catch (error) {
    console.error("Failed to get current user:", error);
    return null;
  }
}
