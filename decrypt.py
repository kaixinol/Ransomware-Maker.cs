from os import path, listdir, remove
from os.path import getsize
from math import ceil
from ctypes import c_byte
from random import randint, seed
from string import ascii_letters

def check_suffix(path):
    global suffix
    return path.endswith(suffix)

def traverse_folders(directory=None):
    global root_directory
    if directory is None:
        directory = root_directory

    try:
        directories = [path.join(directory, d) for d in listdir(directory) if path.isdir(path.join(directory, d))]
        files = [path.join(directory, f) for f in listdir(directory) if path.isfile(path.join(directory, f))]

        for dir_entry in directories:
            if len(dir_entry) >= 3 and '$' not in dir_entry and not path.isfile(dir_entry):
                traverse_folders(dir_entry)

        for file_entry in files:
            if path.isfile(file_entry) and check_suffix(file_entry):
                print(file_entry)
                decrypt_file(file_entry)
    except PermissionError:
        print(f"Access to {directory} is denied.")
    except FileNotFoundError:
        print(f"{directory} directory not found.")

def decrypt_file(file_path):
    global suffix
    try:
        file_size = getsize(file_path)

        if file_size > 512 * 1024 * 1024:
            print("File size exceeds 512MB and will be ignored.")
            return
        else:
            with open(file_path, "rb") as f:
                file_bytes = f.read()
            encrypted_bytes = rc4_encrypt(file_bytes)
            remove(file_path)
            with open(file_path[:-len(suffix)], "wb") as f:
                f.write(encrypted_bytes)

        # print("File decryption successful!")
    except Exception as ex:
        try:
            remove(file_path[:-len(suffix)])
        except:
            pass
        print("An error occurred while decrypting the file:", ex)

def rc4_encrypt(input_data):
    global password
    key_bytes = password.encode("ascii")
    S = list(range(256))
    output = bytearray(len(input_data))

    j = 0
    for i in range(256):
        j = (j + S[i] + key_bytes[i % len(key_bytes)]) % 256
        S[i], S[j] = S[j], S[i]

    x = 0
    y = 0
    for i in range(len(input_data)):
        x = (x + 1) % 256
        y = (y + S[x]) % 256
        S[x], S[y] = S[y], S[x]
        output[i] = input_data[i] ^ S[(S[x] + S[y]) % 256]

    return output

suffix = input("Please enter the encrypted file suffix: ")
root_directory = input("Please enter the directory path to decrypt: ")
password = input("Please enter the password: ")
traverse_folders()