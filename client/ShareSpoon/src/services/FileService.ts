import axios from 'axios';
import { IFile } from '../interfaces/FileInterface';
import { BASE_URL } from '../config/apiConfig';

export const uploadFile = async (fileData: IFile) => {
  const formData = new FormData();
  formData.append("file", fileData.file);
  const response = await axios.post(`${BASE_URL}/api/files/upload`, formData, {
    headers: {
        'Content-Type': 'multipart/form-data'
    }
  });
  return response;
};