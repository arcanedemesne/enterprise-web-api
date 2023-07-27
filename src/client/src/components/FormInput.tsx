import Input from "@mui/joy/Input";
import FormControl from "@mui/joy/FormControl";
import FormLabel from "@mui/joy/FormLabel";

export interface IFormInputProps {
  label?: string | null;
  type: string;
  name: string;
  placeholder?: string | null;
  value: string;
  onChange?: (event: React.ChangeEvent<HTMLInputElement>) => void;
  onKeyUp?: (event: React.KeyboardEvent<HTMLInputElement>) => void;
  hidden?: boolean;
  hasError?: boolean;
}

const FormInput = ({
  label,
  type,
  name,
  placeholder,
  value,
  onChange,
  onKeyUp,
  hidden,
  hasError,
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
          value={value}
          onChange={onChange}
          onKeyUp={onKeyUp}
          sx={{
            display: hidden ? "none" : "auto",
          }}
          error={hasError}
        />
      </FormControl>
    </>
  );
};

export default FormInput;
