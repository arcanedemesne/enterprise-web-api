import Input from "@mui/joy/Input";
import FormControl from "@mui/joy/FormControl";
import FormLabel from "@mui/joy/FormLabel";

export interface IFormInputProps {
  label?: string | null;
  type: string;
  name: string;
  placeholder?: string | null;
  value?: string | null;
  onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
  onKeyUp?: (event: React.KeyboardEvent<HTMLInputElement>) => void;
}

const FormInput = ({
  label, type, name, placeholder, value, onChange, onKeyUp
}: IFormInputProps) => {
  return (
    <>
      <FormControl>
        {label && <FormLabel>{label}</FormLabel>}
        <Input
          // html input attribute
          type={type}
          name={name}
          placeholder={placeholder || ""}
          value={value || ""}
          onChange={onChange}
          onKeyUp={onKeyUp}
        />
      </FormControl>
    </>
  );
};

export default FormInput;
